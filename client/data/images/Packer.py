from PIL import Image
import math
import os
from datetime import date

EMPTY_IMG = "__empty.png"  

#Folders that containing spritesheets
TERRAINPATH = "terrain/"
BUILDINGSPATH = "buildings/"
UNITSPATH = "units/"
MENUPATH = "menu/"
ENEMYPATH = "enemy/"
HELPERPATH = "helper/"
#Outputfolder
OUT = "OUTPUT/"

#array of all Spritesheetfolders
PATHS = (TERRAINPATH, BUILDINGSPATH, UNITSPATH, MENUPATH, ENEMYPATH, HELPERPATH)

#needed variables
IMAGEWIDTH = 107
IMAGEHEIGHT = 72
COLLUM = 10.0
WIDTH = int(COLLUM) * IMAGEWIDTH

#Creates a Spritesheet
def Spritesheet(TMPPATH, COUNT, height):
        offsetx = 0
        offsety = 0
        new_image = Image.open(EMPTY_IMG)
        new_image = new_image.resize((WIDTH, newheight),1)
        for pic in range(1, COUNT+1):
                template = Image.open(TMPPATH+"00"+str(pic)+".png")
                new_image.paste(template, (offsetx, offsety, (offsetx + IMAGEWIDTH), (offsety + IMAGEHEIGHT)))
                offsetx = offsetx + IMAGEWIDTH
                if(offsetx == 1070):
                        offsetx = 0
                        offsety = offsety + IMAGEHEIGHT
        new_image.save(OUT+TMPPATH+"spritesheet.png")
        print ("build "+TMPPATH+" complete")

#mainloop
for item in PATHS:
        i = 0
        path, dirs, files = os.walk(item).next()
        count = len(files)
        height = math.ceil((count / COLLUM)) * IMAGEHEIGHT
        height = int(height)
        Spritesheet(item, count, height)
        i = i + 1

#Combines the Spritesheets together
sheet = Image.open(EMPTY_IMG)
tsheet = Image.open(OUT+TERRAINPATH+"spritesheet.png")
bsheet = Image.open(OUT+BUILDINGSPATH+"spritesheet.png")
usheet = Image.open(OUT+UNITSPATH+"spritesheet.png")
msheet = Image.open(OUT+MENUPATH+"spritesheet.png")
esheet = Image.open(OUT+ENEMYPATH+"spritesheet.png")
hsheet = Image.open(OUT+HELPERPATH+"spritesheet.png")
#gets the new Image Height
gheight = tsheet.height + bsheet.height + usheet.height + msheet.height + esheet.height + hsheet.height

offset = 0;

sheet = sheet.resize((WIDTH, gheight))
sheet.paste(tsheet,(0, offset, WIDTH, offset + tsheet.height))
offset = offset + tsheet.height

sheet.paste(bsheet,(0, offset, WIDTH, offset + bsheet.height))
offset = offset + bsheet.height

sheet.paste(usheet,(0, offset, WIDTH, offset + usheet.height))
offset = offset + usheet.height

sheet.paste(msheet,(0, offset, WIDTH, offset + msheet.height))
offset = offset + msheet.height

sheet.paste(esheet,(0, offset, WIDTH, offset + esheet.height))
offset = offset + esheet.height

sheet.paste(hsheet,(0, offset, WIDTH, offset + hsheet.height))

today = date.today()
today = str(today.year)+str(today.month)+str(today.day)
sheet.save(OUT+"spritesheet"+today+".png")
