import random

class TileType():
    def __init__(self,name,code,can_adjoin_self = True):
        self.name = name
        self.code = code
        self.can_adjoin = []
        if(can_adjoin_self):
            self.can_adjoin.append(self)

    def __str__(self):
        return self.name + " ("+self.code+")"

    def add_adjoiner(self,other_tile_type):
        if(other_tile_type in self.can_adjoin):
            pass
        else:
            self.can_adjoin.append(other_tile_type)

class Tile():
    def __init__(self,row,col,list_of_types):
        self.row = row
        self.col = col
        self.superpositions = list_of_types
        self.collapsed = False

    def restrict_superpositions_by(self,tile):
        pass


class GameMap():
    def __init__(self,width,height):
        self.rows = height
        self.cols = width
        self.tile_types = [TileType("Ocean","~"),#0
                           TileType("Coast","c"),#1
                           TileType("Grassland","-"),#2
                           TileType("Swamp","#"),#3
                           TileType("Jungle","P"),#4
                           TileType("Hill","m"),#5
                           TileType("Slope","^"),#6
                           TileType("Caldera","*",False)]#7
        self.set_can_adjoins_for(self.tile_types[0],self.tile_types[1])
        self.set_can_adjoins_for(self.tile_types[1],self.tile_types[2])
        self.set_can_adjoins_for(self.tile_types[1],self.tile_types[2])
        self.set_can_adjoins_for(self.tile_types[1],self.tile_types[4])
        self.set_can_adjoins_for(self.tile_types[2],self.tile_types[3])
        self.set_can_adjoins_for(self.tile_types[2],self.tile_types[4])
        self.set_can_adjoins_for(self.tile_types[2],self.tile_types[5])
        self.set_can_adjoins_for(self.tile_types[3],self.tile_types[4])
        self.set_can_adjoins_for(self.tile_types[3],self.tile_types[5])
        self.set_can_adjoins_for(self.tile_types[3],self.tile_types[6])
        self.set_can_adjoins_for(self.tile_types[4],self.tile_types[5])
        self.set_can_adjoins_for(self.tile_types[4],self.tile_types[6])
        self.set_can_adjoins_for(self.tile_types[5],self.tile_types[6])
        self.set_can_adjoins_for(self.tile_types[6],self.tile_types[7])
        
        self.tiles = []
        
        
        row = 0
        while(row<self.rows):
            col = 0
            while(col<self.cols):
                self.tiles.append(Tile(row,col,self.tile_types))
                col+=1
            row+=1

    @staticmethod
    def set_can_adjoins_for(tile_type_1, tile_type_2):
        tile_type_1.add_adjoiner(tile_type_2)
        tile_type_2.add_adjoiner(tile_type_1)
    
    def setup_fixed_tiles(self):
        for tile in self.tiles:
            if(tile.row == 0 or tile.row == self.rows-1 or
                tile.col == 0 or tile.col == self.cols-1):

                tile.superpositions = [self.tile_types[0]]
                tile.collapsed = True
            if(tile.row == 12 and tile.col == 12):
                tile.superpositions = [self.tile_types[7]]
                tile.collapsed = True

    def collapse_map(self):
        collapsed = 0
        row = 0
        while(row< self.rows):
            col = 0
            while(col<self.cols):
                tile = self.get_tile_by_coords(row,col)
                if(tile.collapsed):
                    collapsed +=1
                    check_list = self.get_neighbours_of(tile)
                    i=0
                    while(i<len(check_list)):
                        new_superposition = []
                        superpos_count = len(check_list[i].superpositions)
                        j=0
                        while(j<superpos_count):
                            if(check_list[i].superpositions[j] in tile.superpositions[0].can_adjoin):
                                new_superposition.append(check_list[i].superpositions[j])
                            j+=1
                        check_list[i].superpositions = new_superposition
                        i+=1
                col+=1
            row+=1
        return collapsed

    def collapse_tile(self):
        min_superpos = 1000
        lowest_tile_list = []
        row = 0
        while(row<self.rows):
            col = 0
            while(col<self.cols):
                tile = self.get_tile_by_coords(row,col)
                if(tile.collapsed == False):
                    tile_superpos_count = len(tile.superpositions)
                    if(tile_superpos_count<min_superpos):
                        min_superpos = tile_superpos_count
                        lowest_tile_list = [tile]
                    elif(tile_superpos_count == min_superpos):
                        lowest_tile_list.append(tile)
                col+=1
            row+=1
        ltl_len = len(lowest_tile_list)
        if(ltl_len > 0):
            
            target_tile = lowest_tile_list[random.randint(1,ltl_len)-1]
            super_len = len(target_tile.superpositions)
            if(super_len>0):
                resolved_superpos = target_tile.superpositions[random.randint(1,super_len)-1]
                if(resolved_superpos == self.tile_types[0]):  #this ensures that while the coast isn't a square, coast is much more likely than ocean
                    if(random.randint(1,10)>3):
                        resolved_superpos = self.tile_types[1]
                elif(resolved_superpos == self.tile_types[1]): #this ensures that coast has an adjoining collapsed ocean and reverts to grassland if not
                    check_list = self.get_neighbours_of(target_tile,True)
                    missing_ocean = True
                    for tile in check_list:
                        if(tile.collapsed and tile.superpositions[0] == self.tile_types[0]):
                            missing_ocean = False
                    if(missing_ocean):
                        resolved_superpos = self.tile_types[2]
                
                target_tile.superpositions = [resolved_superpos]
                target_tile.collapsed = True


    def get_neighbours_of(self,tile,send_collapsed = False):
        output = []
        if(tile.row!=0):
            nt = self.get_tile_by_coords(tile.row-1,tile.col)
            if(nt.collapsed == False or send_collapsed):
                output.append(nt)
        if(tile.row<self.rows-1):
            nt = self.get_tile_by_coords(tile.row+1,tile.col)
            if(nt.collapsed == False or send_collapsed):
                output.append(nt)
        if(tile.col!=0):
            nt = self.get_tile_by_coords(tile.row,tile.col-1)
            if(nt.collapsed == False or send_collapsed):
                output.append(nt)
        if(tile.col<self.cols-1):
            nt = self.get_tile_by_coords(tile.row,tile.col+1)
            if(nt.collapsed == False or send_collapsed):
                output.append(nt)
        return output

    def get_tile_by_coords(self,row,col):
        index = row*self.cols+col
        return self.tiles[index]

    def draw_map(self):
        last_row = 0;
        row_text = ""
        for tile in self.tiles:
            if(tile.row != last_row):
                last_row = tile.row
                print(row_text)
                row_text = ""
            if(tile.collapsed):
                row_text += tile.superpositions[0].code
            else:
                row_text += str(len(tile.superpositions))
        print(row_text)

    def draw_type_detail(self):
        for terrain_type in self.tile_types:
            print(terrain_type)
            adjoiner_text = ""
            for adjoin in terrain_type.can_adjoin:
                adjoiner_text+= adjoin.code + " "
            print(adjoiner_text)
            

my_map = GameMap(25,25)
my_map.draw_map()
my_map.setup_fixed_tiles()
my_map.draw_map()
my_map.draw_type_detail()

print("collapsing map")
collapsed = 0
prev_collapsed = 0
collapsed_no_change_ticks = 0
while(collapsed < (my_map.rows * my_map.cols)):
    prev_collapsed = collapsed
    collapsed = my_map.collapse_map()
    if(prev_collapsed == collapsed):
        collapsed_no_change_ticks +=1
    else:
        collapsed_no_change_ticks = 0
    my_map.collapse_tile()
    if(collapsed_no_change_ticks>100):
        my_map = GameMap(25,25)
        my_map.setup_fixed_tiles()
my_map.draw_map()
