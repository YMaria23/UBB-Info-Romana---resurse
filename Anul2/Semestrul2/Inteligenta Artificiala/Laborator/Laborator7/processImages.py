# se seteaza calea catre folder
from pathlib import Path

from PIL import Image
import os
import pandas as pd

# REDIMENSIONARE + APLICAREA FILTRULUI SEPIA

# configurare -> din ce folder se pleaca si unde trebuie sa se ajunga
sourcePath = 'rawImages'
destinationPath = 'processedImages16'
os.makedirs(destinationPath, exist_ok=True)

metadata = []

# matricea pentru filtrul sepia (RGB)
sepiaFilter = (0.393, 0.769, 0.189, 0,
                 0.349, 0.686, 0.168, 0,
                 0.272, 0.534, 0.131, 0)

# se colecteaza toate pozele (path-ul lor) (indiferent de subfolder)
allImages = []
for folder, subfoldere, fisiere in os.walk(sourcePath):
    for fisier in fisiere:
        if fisier.lower().endswith('.jpg'):
            allImages.append(os.path.join(folder, fisier))


print("Se creeaza setul de date...")
for i, cale in enumerate(allImages):
    img = Image.open(cale).convert("RGB")
    img = img.resize((16, 16))

    newName = f"img_{i}.jpg"
    newPath = f"{destinationPath}/{newName}"

    # se aplica filtrul numai pe pozele pare
    if i % 2 == 0:
        img = img.convert("RGB", sepiaFilter)
        metadata.append({'filename':newName,'label':'sepia'})
    else:
        metadata.append({'filename':newName,'label':'noSepia'})

    # se salveaza imaginea in folderul destinatie
    img.save(newPath)

print(f"Gata! Ai {len(allImages)} imagini în '{destinationPath}'.")



# ETICHETAREA POZELOR
df = pd.DataFrame(metadata)
# index = False -> nu se mai adauga numerotarea randurilor
df.to_csv("metadata.csv", index=False)

