package org.example.service;

import org.example.domain.Friendship;
import org.example.domain.User;
import org.example.exceptii.AlreadyInRepoException;
import org.example.exceptii.ArgumentException;
import org.example.exceptii.NotInListException;
import org.example.observers.ObservableNew;
import org.example.observers.ObserverNew;
import org.example.repository.PagedRepository;
import org.example.repository.Repository;
import org.example.repository.dto.Page;
import org.example.repository.dto.Pageable;
import org.example.utils.events.ChangeEventType;
import org.example.utils.events.EntityChangeEvent;

import java.util.*;

public class ServiceFriendship implements ObservableNew<EntityChangeEvent> {

    private PagedRepository<User> repoUsers;
    private PagedRepository<Friendship> repoFriendship;
    private List<ObserverNew<EntityChangeEvent>> observers = new ArrayList<>();

    public ServiceFriendship(PagedRepository<User> repoUsers, PagedRepository<Friendship> repoFriendship) {
        this.repoUsers = repoUsers;
        this.repoFriendship = repoFriendship;
    }

    /**
     * Adauga o prietenie in lista
     * @param id1 - id-ul primului utilizator
     * @param id2 - id-ul celui de-al doilea utilizator
     * @throws ArgumentException daca cel putin unul dintre id-urile utilizatorilor este null
     *        AlreadyInRepoException daca exista deja respectiva prietenie
     *        NotInListException daca nu exista unul dintre utilizatori
     * @return Optional ce contine prietenia adaugata, daca aceasta nu se afla deja in lista
     * */
    public Optional<Friendship> addFriendships(Long id1, Long id2){
        if(id1 == null || id2 == null)
            throw new ArgumentException("The id(s) can not be null");

        Optional<User> util1 = this.repoUsers.findOne(id1);
        Optional<User> util2 = this.repoUsers.findOne(id2);

        if(util1.isEmpty() || util2.isEmpty())
            throw new NotInListException("Unul sau ambii utilizatori nu exista!");

        User u1 = util1.get();
        User u2 = util2.get();

        if(findFriendshipId(id1, id2) != null)
            throw new AlreadyInRepoException("Exista deja aceasta prietenie");

        Friendship prietenie_noua = new Friendship(id1,id2);
        Optional<Friendship> newFriendship = repoFriendship.save(prietenie_noua);
        newFriendship.ifPresent(newFriendship1 -> notifyObservers(new EntityChangeEvent(ChangeEventType.ADD,newFriendship1)));
        rebuildFriendLists();

        return newFriendship;
    }

    /**
     * Actualizeaza o prietenie in lista
     * @param id1 - id-ul primului utilizator
     * @param id2 - id-ul celui de-al doilea utilizator
     * @throws ArgumentException daca cel putin unul dintre id-urile utilizatorilor este null
     *        NotInListException daca nu exista unul dintre utilizatori
     * @return Optional - cu prietenia actualizata, daca aceasta se afla in lista
     *                  - empty daca prietenia nu exista
     * */
    public Optional<Friendship> updateFriendships(Long old1,Long old2,Long id1, Long id2){
        if(id1 == null || id2 == null || old1 == null || old2 == null)
            throw new ArgumentException("The id(s) can not be null");

        Optional<User> util1 = this.repoUsers.findOne(id1);
        Optional<User> util2 = this.repoUsers.findOne(id2);

        if(util1.isEmpty() || util2.isEmpty())
            throw new NotInListException("Unul sau ambii utilizatori nu exista!");

        User u1 = util1.get();
        User u2 = util2.get();

        Long idFriendship = findFriendshipId(old1, old2);
        if(idFriendship == null)
            return Optional.empty();

        Friendship prietenieNoua = new Friendship(id1,id2);
        prietenieNoua.setId(idFriendship);

        Optional<Friendship> newFriendship = repoFriendship.update(prietenieNoua);
        newFriendship.ifPresent(newFriendship1 -> notifyObservers(new EntityChangeEvent(ChangeEventType.ADD,newFriendship1)));
        rebuildFriendLists();

        return newFriendship;
    }

    /**
     * Metoda gaseste o prietenie dupa utilizatori
     * @param id1 - primul utilizator (id-ul)
     * @param id2 - cel de-al doilea utilizator (id-ul)
     * @return id-ul prieteniei, daca aceasta exista
     *         null, in caz contrar
     * */

    public Long findFriendshipId(Long id1, Long id2){
        for (Friendship f : repoFriendship.findAll()) {
            boolean match = (f.getUser1().equals(id1) && f.getUser2().equals(id2))
                    || (f.getUser1().equals(id2) && f.getUser2().equals(id1));
            if (match) return f.getId();
        }
        return null;
    }


    /**
     * Metoda elimina o prietenie din lista
     * @param id1 - id-ul primului utilizator
     * @param id2 - id-ul celui de-al doilea utilizator
     * @throws ArgumentException daca unul dintre id-uri este null
     * @return Optional - cu prietenia, daca aceasta exista si a fost eliminata
     *                  - empty, daca nu s-a gasit prietenia in lista
     * */
    public Optional<Friendship> removeFriendships(Long id1, Long id2){
        if(id1 == null || id2 == null)
            throw new ArgumentException("The id(s) can not be null");

        //User u1 = repoUsers.findOne(id1);
        //User u2 = repoUsers.findOne(id2);

        Long id = findFriendshipId(id1,id2);
        if(id == null)
            throw new NotInListException("Friendship is not in repository");

        Optional<Friendship> deletedFriendship = repoFriendship.delete(id);
        deletedFriendship.ifPresent(m ->
                notifyObservers(new EntityChangeEvent<>(ChangeEventType.DELETE, null, m)));
        rebuildFriendLists();

        return deletedFriendship;
    }


    /**
     * Algoritm clasic de DFS recursiv
     * @param comunitateCurenta - reprezinta componenta conexa curenta
     * @param vizitat - lista de utilizatori deja vizitati
     * @param comunitati - lista cu toate componentele conexe
     * */
    private void DFS(Long comunitateCurenta, Set<Long> vizitat, List<Long> comunitati) {
        // marchează ca vizitat
        vizitat.add(comunitateCurenta);
        comunitati.add(comunitateCurenta);

        // obține toți prietenii userului curent
        List<Long> prieteni = new ArrayList<>();
        for (Friendship f : repoFriendship.findAll()) {
            if(Objects.equals(f.getUser1(), comunitateCurenta))
                prieteni.add(f.getUser2());
            else if(Objects.equals(f.getUser2(), comunitateCurenta))
                prieteni.add(f.getUser1());
        }
        //List<User> prieteni = comunitateCurenta.getPrieteni();
        //if (prieteni == null) return;

        for (Long prieten : prieteni) {
            if (!vizitat.contains(prieten)) {
                DFS(prieten, vizitat, comunitati);
            }
        }
    }

    /**
     * Metoda determina comunitatile
     * @return o lista de componente conexe ce reprezinta comunitati
     * */
    private List<List<Long>> determinaComunitatile() {
        Set<Long> vizitat = new HashSet<>();
        List<List<Long>> comunitati = new ArrayList<>();

        for (User u : repoUsers.findAll()) {
            if (!vizitat.contains(u.getId())) {
                List<Long> comunitate = new ArrayList<>();
                DFS(u.getId(), vizitat, comunitate);
                comunitati.add(comunitate);
            }
        }
        return comunitati;
    }

    /**
     * Algoritm de BFS modificat in stilul Bellman Ford
     * @param start - utilizatorul cu care incepem cautarea
     * */
    private Map<Long, Integer> BFS(Long start) {
        Map<Long, Integer> dist = new HashMap<>();
        Queue<Long> queue = new LinkedList<>();

        dist.put(start, 0);
        queue.add(start);

        while (!queue.isEmpty()) {
            Long current = queue.poll();
            int currentDist = dist.get(current);

            List<Long> prieteni = new ArrayList<>();
            for (Friendship f : repoFriendship.findAll()) {
                if(Objects.equals(f.getUser1(), current))
                    prieteni.add(f.getUser2());
                else if(Objects.equals(f.getUser2(), current))
                    prieteni.add(f.getUser1());
            }

            for (Long prieten : prieteni) {
                if (!dist.containsKey(prieten)) {
                    dist.put(prieten, currentDist + 1);
                    queue.add(prieten);
                }
            }
        }
        return dist;
    }


    /**
     * Metoda determina dimensiunea (maxima) unei comunitati
     * @param comunitate - lista de utilizatori (comunitatea)
     * */
    private int  determinareDimensiuneComunitate(List<Long> comunitate) {
        int dimensiune = 0;

        for (Long u : comunitate) {
            Map<Long, Integer> dist = BFS(u);
            int localMax = Collections.max(dist.values());
            dimensiune = Math.max(dimensiune,localMax);
        }

        return dimensiune;
    }

    /**
     * Metoda care determina cea mai sociabila comunitate (compunenta conexa cu dimensiunea cea mai mare)
     * @return o lista de utilizatori ce reprezinta cea fac parte din cea sociabila comunitate
     * */
    public List<Long> ceaMaiSociabilaComunitate() {
        List<List<Long>> comunitati = determinaComunitatile();
        List<Long> comunitateSociabila = null;
        int dimensiuneMax = -1;

        for (List<Long> comunitate : comunitati) {
            int dimensiune = determinareDimensiuneComunitate(comunitate);

            if (dimensiune > dimensiuneMax) {
                dimensiuneMax = dimensiune;
                comunitateSociabila = comunitate;
            }
        }

        System.out.println("Diametrul maxim: " + dimensiuneMax);
        return comunitateSociabila;
    }

    /**
     * Metoda care determina nr de componente conexe (comunitati)
     * @return nr de comp conexe
     * */
    public int nrComunitati() {
        List<List<Long>> comunitati = determinaComunitatile();
        return comunitati.size();
    }

    /**
     * @return lista de friendships
     * */
    public List<Friendship> listFriendships(){
        return (ArrayList)repoFriendship.findAll();
    }

    /**
     * @return lista de friendships paginata
     * */
    public Page<Friendship> findFriendshipsOnPage(Pageable pageable){
        return repoFriendship.findAllOnPage(pageable);
    }

    public void rebuildFriendLists() {
        // goliți listele de prieteni
        for (User u : repoUsers.findAll()) {
            u.getPrieteni().clear();
        }

        // în loc să recreezi obiecte Friendship, folosește doar userii
        List<Friendship> allFriendships = (ArrayList)repoFriendship.findAll();
        for (Friendship f : allFriendships) {
            Optional<User> a = repoUsers.findOne(f.getUser1());
            Optional<User> b = repoUsers.findOne(f.getUser2());
            if (a.isPresent() && b.isPresent()) {
                a.get().getPrieteni().add(b.get());
                b.get().getPrieteni().add(a.get());
            }
        }
    }

    @Override
    public void addObserver(ObserverNew<EntityChangeEvent> e) {
        observers.add(e);
    }

    @Override
    public void removeObserver(ObserverNew<EntityChangeEvent> e) {
        observers.remove(e);
    }

    @Override
    public void notifyObservers(EntityChangeEvent t) {
        observers.stream().forEach(observer -> observer.update(t));
    }
}
