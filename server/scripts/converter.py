# coding: utf-8


import math
import os
import os.path

import json
import sys
import threading
import random

import Image


def get_file_path(lat_nr, lon_nr, ending, special="", folder="World"):
    folder_multiplier = 16
    
    if special:
        special = "-" + special
    if not os.path.exists("%s/%s" % (folder, str(lat_nr / folder_multiplier ),)):
        os.system("mkdir %s/%s" % (folder, str(lat_nr / folder_multiplier ),))
    if not os.path.exists("%s/%s/%s" % (folder, str(lat_nr / folder_multiplier ), str(lon_nr / folder_multiplier ),)):
        os.system("mkdir %s/%s/%s" % (folder, str(lat_nr / folder_multiplier ), str(lon_nr / folder_multiplier ),))

    return "%s/%s/%s/germany-%s-%s%s.%s" % (folder, str(lat_nr / folder_multiplier ), str(lon_nr / folder_multiplier ), lat_nr, lon_nr, special, ending)



def as_position(lat, lon):
    zoom = 40075036.0 / cell_pixel 
    x = float((float(lon) + 180.0) / 360.0 * (zoom))
    y = float((1.0 - math.log(math.tan(float(lat) * math.pi / 180.0) + \
    1.0 / math.cos(lat * math.pi / 180.0)) / math.pi) / 2.0 * (zoom))

    return x, y

def as_lat_lon (x, y):
    zoom = 40075036.0 / cell_pixel
    n = math.pi - ((2.0 * math.pi * float(y)) / zoom)

    lon = float((float(x) / zoom * 360.0) - 180.0)
    lat = float(180.0 / math.pi * math.atan(math.sinh(n)))

    return (lat, lon)
    
def as_region_position(x, y):
    return int(x / region_size) * region_size, int(y / region_size) * region_size
    
 

class Vector3:
    def __init__(self, x, y, z = 0):
        self.x = x
        self.y = y
        self.z = z
    
    def __add__(self, other):
        return Vector3(self.x + other.x, self.y + other.y, self.z + other.z)
        
    def __sub__(self, other):
        return Vector3(self.x - other.x, self.y - other.y, self.z - other.z)
    
    def __len__(self):
        return self.x * self.x + self.y * self.y + self.z * self.z
    
    
    def __hash__(self):
        return self.x * 255 * 255 + self.y * 255 + self.z
    
    def __eq__(self, other):
        return self.x == other.x and self.y == other.y and self.z == other.z
    
    def __str__(self):
        return "(%s, %s, %s)" % (self.x, self.y, self.z)
Vector = Vector3



def crop_osm(lat, lon, lat_end, lon_end, lat_nr, lon_nr):
    path = get_file_path(lat_nr, lon_nr, "osm")
    if os.path.exists(path): return

    
    command = "osmosis --read-xml file=germany-part.osm --bounding-box \
 top=%s bottom=%s left=%s right=%s  completeRelations=no completeWays=no \
 cascadingRelations=yes clipIncompleteEntities=false \
 --buffer bufferCapacity=12000 --write-xml file=%s" % (lat, lat_end, lon, lon_end, path)
    #command = "wget http://overpass-api.de/api/map?bbox=%s,%s,%s,%s" % (lon, lat_end, lon_end, lat)
    print "Command: ", command
    os.system(command)

def convert_osm_to_svg(lat_nr, lon_nr):
    path = get_file_path(lat_nr, lon_nr, "svg")
    if os.path.exists(path): return
    path = get_file_path(lat_nr, lon_nr, "osm")
    
    command = "./osmarender -r stylesheets/stylesheet_9.xml %s" % (path,)
    print "Command: ", command
    os.system(command)

def convert_svg_to_png(lat_nr, lon_nr):
    path_png = get_file_path(lat_nr, lon_nr, "png")
    path_svg = get_file_path(lat_nr, lon_nr, "svg")
    if os.path.exists(path_png): return


    command = "rsvg-convert --width=%s --height=%s --format=png --output  %s  %s" % (str((region_pixel + border_pixel)), str((region_pixel + border_pixel)), path_png, path_svg)
    print "Command: ", command
    os.system(command)



def convert_png_to_json(lat_nr, lon_nr):
    path_json = get_file_path(lat_nr, lon_nr, "json", folder="json")
    if os.path.exists(path_json): return

    path_png  = get_file_path(lat_nr, lon_nr, "png")    
    img = Image.open(path_png)
        
    terrain = [] 
    
    for x in range(0, img.size[0], int(cell_pixel)): #cell_pixel  * 0.75
        terrain_row = []
        for y in range(0, img.size[1], cell_pixel):            
            possible_terrains = []
            if x % (cell_pixel * 2) != 0:
                y += cell_pixel / 2
            position = Vector(x, y)
            
            for neighbour in neighbours:
                neighbour_position = neighbour + position
                
                try:
                    color = img.getpixel((neighbour_position.x, neighbour_position.y))
                except:
                    continue
                #print color
                if len(color) == 4:
                    r, g, b, a = color
                else:
                    r, g, b = color

                typ_info = Vector3(r, g, b)
                if typ_info in terrain_types:
                    possible_terrains.append(typ_info)

            color = img.getpixel((position.x, position.y))
            #print color
            if len(color) == 4:
                r, g, b, a = color
            else:
                r, g, b = color
            typ_info = Vector3(r, g, b)

            length = 99999999
            choosen = None 
            for typ in possible_terrains or terrain_types:
                if len(typ - typ_info) < length:
                    length = len(typ - typ_info)
                    choosen = typ
            terrain_id = terrain_types[choosen][0]
            terrain_row.append(terrain_id)
        terrain.append(terrain_row)
    infos = terrain
#             "Number": (lat_nr, lon_nr),
#             "LatLon": (lat, lon),
#             "Terrain" : terrain
#            }
    
    
    with open(path_json, "wb+") as handle:
        handle.write(json.dumps(infos))
    
def crop_png(lat_nr, lon_nr):
    path = get_file_path(lat_nr, lon_nr, "png")
    try:
        image = Image.open("%s" % (path,))
    except:
        print "FEHLER:", lat_nr, lon_nr
        return
    new_image = image.crop((border_pixel / 2, border_pixel / 2, image.size[0] - border_pixel / 2, image.size[1] - border_pixel / 2))
    path = get_file_path(lat_nr, lon_nr, "png", "cutted")
    new_image.save("%s" % path)
    return True
    
def cut_pngs_into_tiles(lat_nr, lon_nr):
    path = get_file_path(lat_nr, lon_nr, "png", "cutted")
    image = Image.open("%s" % (path,))

    max_x = int(image.size[0] / real_region_pixel)
    max_y = int(image.size[1] / real_region_pixel)
    tiles_latlon = []
    for x in range(max_x):
        for y in range(max_y):
            new_lat_nr = (lat_nr / real_region_size) + x
            new_lon_nr = (lon_nr / real_region_size) + y
            tiles_latlon.append((new_lat_nr, new_lon_nr))
            path = get_file_path(new_lat_nr, new_lon_nr, "png")
            if os.path.isfile(path): continue
            new_image = image.crop((x * real_region_pixel, y * real_region_pixel, (x + 1) * real_region_pixel, (y + 1) * real_region_pixel))
            new_image.save("%s" % path)
    return tiles_latlon
#    new_image = image.crop((border_pixel / 2, border_pixel / 2, region_pixel + border_pixel / 2, region_pixel + border_pixel / 2))

def convert_json_to_image(lat_nr, lon_nr):    

    path_png  = get_file_path(lat_nr, lon_nr, "png", folder="png")
    path_json = get_file_path(lat_nr, lon_nr, "json", folder="json")
    
    with open(path_json, "rb+") as handler:
        terrains = json.load(handler)

    #if os.path.isfile(path_png): return
    tilesize_x = 72  * 0.75
    tilesize_y = 72
    
    new_img_size_x = int(len(terrains)  * tilesize_x + tilesize_x / 3)
    new_img_size_y = int(len(terrains[1]) * tilesize_y  + tilesize_y / 3)
    newimg = Image.new("RGB", (new_img_size_x, new_img_size_y), "white")

    for x in range(len(terrains)):
        for y in range(len(terrains[x])):
            choosen = [possible for possible in terrain_types.values() if possible[0] == terrains[x][y]] or [terrain[Vector3(0xFF, 0xFF, 0xFF)]]            
            
            if x % 2 != 0:
                y += 0.5
            
            terrain_image = choosen[0][3];#, typ_info, length
            start_pos = int(x * tilesize_x), int(y * tilesize_y)
            newimg.paste(terrain_image, start_pos, terrain_image)

    newimg.save(path_png)
    

def worker(regions):
    regions.reverse()
    #random.shuffle(regions)
    while len(regions):
        lat, lon, lat_end, lon_end, lat_nr, lon_nr = regions.pop()
        
        crop_osm(lat, lon, lat_end, lon_end, lat_nr, lon_nr)   
        convert_osm_to_svg(lat_nr, lon_nr)
        print "len", len(regions)
        convert_svg_to_png(lat_nr, lon_nr)
        crop_png(lat_nr, lon_nr)
        new_latlon_nrs = cut_pngs_into_tiles(lat_nr, lon_nr)
        for new_lat_nr, new_lon_nr in new_latlon_nrs:
            convert_png_to_json(new_lat_nr, new_lon_nr)
            #convert_json_to_image(new_lat_nr, new_lon_nr)


terrain_types = {
    Vector3(0x50, 0x90, 0xF0) : [0, "Water", "coast-tropical-tile.png"],
    Vector3(0x80, 0x18, 0x18) : [1, "Buildings", "wood-tan.png"], #"dwarven-castle-floor.png"
    Vector3(0x00, 0x77, 0x00) : [2, "Woods", "forested-hills-tile.png"],
    Vector3(0xB0, 0xE4, 0x40) : [3, "Grassland", "green.png"],
    Vector3(0xEB, 0xFF, 0xF2) : [4, "Fields", "farm-veg-spring-icon.png"],
    Vector3(0xB0, 0xB0, 0xB0) : [5, "Streets", "road-clean.png"],
    Vector3(0xFF, 0x44, 0xFF) : [6, "Not defined yet, everything else", "lava.png"],
    Vector3(0xFF, 0x00, 0x00) : [7, "Forbidden", "lava.png"],
    Vector3(0xE9, 0xE9, 0xE9) : [8, "Town", "path.png"],
    Vector3(0xFA, 0xFA, 0xFF) : [9, "Gletscher", "lava.png"],
    Vector3(0xEE, 0xCC, 0x55) : [10, "Beach", "beach.png"],
    Vector3(0xA0, 0xD0, 0x30) : [11, "Park", "green8.png"],
    Vector3(0xFF, 0xFF, 0xFF) : [12, "Invalid", "lava.png"]
}

for number in terrain_types:
    imagename = terrain_types[number][2]
    terrain_types[number].append(Image.open("images/%s" % imagename)) 


thread_count = 4

start_lat = 51.45
start_lon = 10.55        

end_lat = 50.55
end_lon = 11.45

cell_pixel = 4 # meters
region_size = 512 # cells per regions
real_region_size = 32
real_region_pixel = cell_pixel * real_region_size
region_pixel = region_size * cell_pixel
border_size = region_size / 2  # cells
border_pixel = border_size * cell_pixel 

start_x, start_y  = as_region_position(*as_position(start_lat, start_lon))
end_x, end_y  = as_region_position(*as_position(end_lat, end_lon))


neighbours = [Vector(0, 0), Vector(0, 1), Vector(1, 0),  Vector(-1, 0), Vector(0, -1),
              Vector(1, 1),Vector(-1, -1), Vector(1, -1), Vector(-1, 1),
              Vector(-2, 2), Vector(-2, 1), Vector(-2, 0), Vector(-2, -1), Vector(-2, -2),
              Vector(2, 2), Vector(2, 1), Vector(2, 0), Vector(2, -1), Vector(2, -2),
              Vector(1, -2), Vector(0, -2), Vector(-1, -2), 
              Vector(-1, 2), Vector(0, 2), Vector(1, 2)
              ]

regions = []


def main():
    for y in xrange(start_y, end_y, region_size):
        for x in xrange(start_x, end_x, region_size):
            x1 = x - border_size / 2
            y1 = y - border_size / 2
            x2 = x + border_size / 2
            y2 = y + border_size / 2
            
            
            region_start_lat, region_start_lon = as_lat_lon(x1, y1)
            region_end_lat, region_end_lon = as_lat_lon(x2 + region_size, y2 + region_size)
            regions.append((region_start_lat, region_start_lon, region_end_lat, region_end_lon, x, y))
    threads = []
    
    for thread_nr in range(thread_count):
        t = threading.Thread(target=worker, args=(regions, ))
        t.start()
        threads.append(t)
    
    # wait for the threads to finish
    for thread in threads:
        thread.join()


pos = as_position(50.9849, 11.0442)
#lat_lon = as_lat_lon(5316738.328188333, 3354722.9170058006)
print pos
print int(pos[0] / real_region_size), int(pos[1] / real_region_size)
print int(pos[0] % real_region_size), int(pos[1] % real_region_size)
main()

#convert_png_to_json(41535, 26184)
#convert_json_to_image(41535, 26184)

"""

for x in range(startx, endx, stepx):
    for y in range(starty, endy, stepy):
        
        if (x - startx) % (stepx * 2) != 0:
            y += stepy / 2
            
        r, g, b, a = img.getpixel((x, y))
        typ_info = Vector3(r, g, b)
        length = 99999999
        choosen = None 
        for typ in terrains:
            if len(typ - typ_info) < length:
                length = len(typ - typ_info)
                choosen = typ


"""
