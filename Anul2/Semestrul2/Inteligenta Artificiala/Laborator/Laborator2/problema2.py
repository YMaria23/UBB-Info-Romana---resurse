#Se dau mai multe imagini (salvate in folder-ul "data/images"). Se cere:
from PIL import Image,ImageFilter
import glob, os

from matplotlib import pyplot as plt

def show_all_images(images):
    col = 4
    rows = (len(images) + col - 1) // col

    # definim cat de mare se va afisa imaginea (inch)
    plt.figure(figsize=(8, 8))

    for index, img in enumerate(images):
        plt.subplot(rows, col, index + 1)
        if img.mode == "L":
            plt.imshow(img, cmap="gray")
        else:
            plt.imshow(img)
        plt.axis('off')

    plt.show()


#sa se vizualizeze una din imagini
with Image.open("images/BERT.png") as img:
    img.show()

#daca imaginile nu aceeasi dimensiune, sa se redimensioneze toate la 128 x 128 pixeli si sa se vizualizeze imaginile intr-un cadru tabelar.
size = 128, 128
extensions = ['.png','.jpg', '.jpeg','.webp']
files = []

images = []

for ext in extensions:
    files.extend(glob.glob('images/*'+ext))

for infile in files:
    #imparte in denumirea imaginii (file) si extensia (ext)
    file, ext = os.path.splitext(infile)

    with Image.open(infile) as im:
        #redimensionare
        img = im.resize(size)
        #salvare
        images.append(img)

#dupa ce s-a creat lista de imagini redimensionate, urmeaza sa fie afisate pe ecran
show_all_images(images)

#sa se transforme imaginile in format gray-levels si sa se vizualizeze
grey_images = []

for infile in files:
    #imparte in denumirea imaginii (file) si extensia (ext)
    file, ext = os.path.splitext(infile)

    with Image.open(infile) as im:
        #format grey-level
        img = im.convert('L')
        grey_images.append(img)

show_all_images(grey_images)

#sa se blureze o imagine si sa se afiseze in format "before-after"
img = Image.open("images/Karpaty.jpg")
blur = img.filter(ImageFilter.BLUR)

#asemanator cu afisarea sub forma de tabel
#fig -> figura
#axes -> lista de subplots
fig, axes = plt.subplots(1, 2, figsize=(10, 5))
fig.canvas.manager.set_window_title("Blur Comparison")
axes[0].imshow(img)
axes[0].set_title("Before")
axes[0].axis('off')

axes[1].imshow(blur)
axes[1].set_title("After")
axes[1].axis('off')

plt.show()

#sa se identifice muchiile intr-o imagine si sa se afiseze in format "before-after"
img = Image.open("images/Karpaty.jpg")
edges = img.filter(ImageFilter.FIND_EDGES)

fig1, axes = plt.subplots(1, 2, figsize=(10, 5))
fig1.canvas.manager.set_window_title("Edges Comparison")
axes[0].imshow(img)
axes[0].set_title("Before")
axes[0].axis('off')

axes[1].imshow(edges)
axes[1].set_title("After")
axes[1].axis('off')

plt.show()
