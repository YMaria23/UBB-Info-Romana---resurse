import os
import xml.etree.ElementTree as ET

def extrage_strofe (file_path):

    toate_poeziile = []

    for fisier in os.listdir(file_path):
        if fisier.endswith(".xml"):
            path = os.path.join(file_path, fisier)

            tree = ET.parse(path)
            root = tree.getroot()

            # pentru inlocuire
            ns = {"tei": "http://www.tei-c.org/ns/1.0"}

            # se cauta toate strofele din poezie
            '''
            for lg in root.findall('.//tei:lg', ns):
                # se obtin liniile din strofa
                linii = [l.text.strip() for l in lg.findall('tei:l', ns) if l.text]

                if len(linii) >= 2:
                    # se salveaza sub forma de dictionar
                    # fiecare strofa va avea vers de inceput si restul versurilor
                    toate_strofele.append({
                        "fisier": fisier,
                        "primul_vers": linii[0],
                        "restul_strofei": linii[1:]
                    })
            '''
            for poezie_element in root.findall('.//tei:lg[@type="poem"]', ns):
                titlu = poezie_element.find('tei:head', ns)
                titlu_text = titlu.text.strip() if titlu is not None else "Untitled"

                strofe_poezie = []

                # se cauta strofele din interiorul poeziei
                for strofa_element in poezie_element.findall('.//tei:lg[@type="stanza"]', ns):
                    linii = [l.text.strip() for l in strofa_element.findall('tei:l', ns) if l.text]
                    if linii:
                        strofe_poezie.append({
                            "primul_vers": linii[0],
                            "restul": linii[1:]
                        })

                # daca nu exista strofe propriu-zise, vom considera ca toata poezia are o singura strofa
                if not strofe_poezie:
                    linii_directe = [l.text.strip() for l in poezie_element.findall('tei:l', ns) if l.text]
                    if linii_directe:
                        strofe_poezie.append({"primul_vers": linii_directe[0], "restul": linii_directe[1:]})

                if strofe_poezie:
                    toate_poeziile.append({
                        "fisier": fisier,
                        "titlu": titlu_text,
                        "strofe": strofe_poezie
                    })

    return toate_poeziile