# -*- coding: utf-8 -*-

from PIL import Image
import os.path


PATH = "/home/leto/Projekte/MonoDevelop/ascendancy/client/data/images/"
EMPTY_IMG = "/home/leto/Projekte/MonoDevelop/ascendancy/client/data/images/__empty.png"
NEW_SIZE = (175, 150)
OFFSET = (44, 0)


empty_image = Image.open(EMPTY_IMG)
empty_image = empty_image.resize((NEW_SIZE[0] + OFFSET[0], NEW_SIZE[1] + OFFSET[1]))

for root, folders, files in os.walk(PATH):
    for file in files:
        if file.startswith("RESIZED"): continue
        template = empty_image.copy()
        img = Image.open(os.path.join(root, file))
        new_img = img.resize(NEW_SIZE)

        template.paste(new_img, ((0, 0, NEW_SIZE[0], NEW_SIZE[1])))
        template = template.offset(*OFFSET)

        template.save(os.path.join(root, "RESIZED-ISOMETRIC-" + file.split('.')[0] + ".png"), "png")
    break


    #m.resize(size, filter) ⇒ i