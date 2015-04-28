# -*- coding: utf-8 -*-

from PIL import Image
import os.path


PATH = "/home/leto/Projekte/MonoDevelop/ascendancy/client/data/images/"
NEW_SIZE = (175, 150)



for root, folders, files in os.walk(PATH):
    for file in files:
        im = Image.open(os.path.join(root, file))
        print os.path.join(root, file)
        new_img = im.resize(NEW_SIZE)
        new_img.save(os.path.join(root, "RESIZED-" + file.split('.')[0] + ".png"), "png")
    break


    #m.resize(size, filter) ⇒ i