from PIL import Image
import math
import os
from datetime import date

EMPTY_IMG = "__empty.png"  

#Folders that containing spritesheets
PATHS = {"unit_72": os.path.join("animations", "unit_warrior"),
         }
#Outputfolder
OUTPUT_PATH = os.path.join("OUTPUT", "animations")

#needed variables
MAX_COLUMNS = 10.0


def EnsureDir(directory):
    if not os.path.exists(directory):
        print directory
        os.makedirs(directory)

def CreatePlist(output_file, image_name, plist):
    template = """<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple Computer//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
    <dict>
        <key>frames</key>
        <dict>
        %s
        </dict>
        <key>metadata</key>
        <dict>
        %s
        </dict>
    </dict>
</plist>
"""


    sprite_template = """
            <key>%s</key>
            <dict>
                <key>aliases</key>
                <array/>
                <key>spriteOffset</key>
                <string>{0,0}</string>
                <key>spriteSize</key>
                <string>{%s,%s}</string>
                <key>spriteSourceSize</key>
                <string>{%s,%s}</string>
                <key>textureRect</key>
                <string>{{%s,%s},{%s,%s}}</string>
                <key>textureRotated</key>
                <false/>
            </dict>
"""
    sprites = []
    for spritename in plist:
        pos = plist[spritename]
        sprites.append(sprite_template % (spritename, pos[2], pos[3], pos[2], pos[3], pos[0], pos[1], pos[2], pos[3])) 

    max_x = max([size_x for x, y, size_x, size_y in plist.values()])
    max_y = max([size_y for x, y, size_x, size_y in plist.values()])
    metadata = """<key>format</key>
            <integer>3</integer>
            <key>realTextureFileName</key>
            <string>%s</string>
            <key>size</key>
            <string>{%s,%s}</string>
            <key>textureFileName</key>
            <string>%s</string>
""" % (image_name, max_x, max_y, image_name)
    
    with open(output_file, "wb+") as file:
        file.write(template % (''.join(sprites), metadata))

#Creates a Spritesheet
def CreateSpritesheet(name, path):
    print '#'*10, path, '#'*10
    plist = {}
    sheet = Image.open(EMPTY_IMG)

    size_x = 0
    size_y = 0
    
    offset_x = 0
    for root, dirs, files in os.walk(path):
        offset_y = 0
        for file_index  in range(len(files)):
            if file_index % MAX_COLUMNS == MAX_COLUMNS - 1:
                offset_y = size_y
                offset_x = 0
                
            filename = files[file_index]
            sprite = Image.open(os.path.join(root, filename))
            
            sprite_size_x, sprite_size_y = sprite.size
            new_offset_x = offset_x + sprite_size_x
            new_offset_y = offset_y + sprite_size_y

            size_y = max(size_y, new_offset_y)
            size_x = max(size_x, new_offset_x)
            
            sheet = sheet.crop((0, 0, size_x, size_y))
            sheet.paste(sprite, (offset_x, offset_y, new_offset_x, new_offset_y))
            
            plist[filename] = (offset_x, offset_y, sprite_size_x, sprite_size_y,)
            offset_x = new_offset_x
            print filename

    directory = os.path.join(OUTPUT_PATH)
    EnsureDir(OUTPUT_PATH)
    image_name = "%s.png" % name
    sheet.save(os.path.join(OUTPUT_PATH, image_name))
    CreatePlist(os.path.join(OUTPUT_PATH, "%s.plist" % name), image_name, plist);

#mainloop
for name in PATHS:
    CreateSpritesheet(name, PATHS[name])
    #break
