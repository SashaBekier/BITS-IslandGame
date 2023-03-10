import random

class Terrain():
    def __init__(self,name,code):
        self.name = name
        self.code = code
        self.constraints = []

class Constraint():
    def __init__(self,terrain,weight = 1):
        self.terrain = terrain
        self.weight = weight

class MapTile():
    def __init__(self,row,col,inital_superposition_list):
        self.row = row
        self.col = col
        self.superpositions = inital_superposition_list
        self.collapsed = False
        self.terrain = None

    def __str__(self):
        if(self.terrain == None):
            str_rep = str(len(self.superpositions)) + " SPS"
        else:
            str_rep = self.terrain.name 
        return str_rep + " ("+str(self.row)+","+str(self.col)+") "

    def set_superpositions(self, superpositions):
        self.superpositions = superpositions
        if (len(self.superpositions)==1):
            self.collapsed = True
            self.terrain = self.superpositions[0]

class GameMap():
    def __init__(self,width,height):
        self.rows = height
        self.cols = width
        self.tiles = []
        self.terrains = [Terrain("Ocean"," "),    #0
                           Terrain("Coast","c"),    #1
                           Terrain("Grassland","-"),#2
                           Terrain("Swamp","#"),    #3
                           Terrain("Jungle","P"),   #4
                           Terrain("Hill","m"),     #5
                           Terrain("Slope","^"),    #6
                           Terrain("Caldera","*")]  #7
        self.terrains[0].constraints.append(Constraint(self.terrains[0],1))
        self.terrains[0].constraints.append(Constraint(self.terrains[1],3))

        self.terrains[1].constraints.append(Constraint(self.terrains[0],0))
        self.terrains[1].constraints.append(Constraint(self.terrains[1],0))
        self.terrains[1].constraints.append(Constraint(self.terrains[2],6))
        self.terrains[1].constraints.append(Constraint(self.terrains[3],2))
        self.terrains[1].constraints.append(Constraint(self.terrains[4]))

        self.terrains[2].constraints.append(Constraint(self.terrains[1],0))
        self.terrains[2].constraints.append(Constraint(self.terrains[2],6))
        self.terrains[2].constraints.append(Constraint(self.terrains[3]))
        self.terrains[2].constraints.append(Constraint(self.terrains[4]))
        self.terrains[2].constraints.append(Constraint(self.terrains[5]))

        self.terrains[3].constraints.append(Constraint(self.terrains[1],0))
        self.terrains[3].constraints.append(Constraint(self.terrains[2]))
        self.terrains[3].constraints.append(Constraint(self.terrains[3],6))
        self.terrains[3].constraints.append(Constraint(self.terrains[4]))
        self.terrains[3].constraints.append(Constraint(self.terrains[5]))

        self.terrains[4].constraints.append(Constraint(self.terrains[1],0))
        self.terrains[4].constraints.append(Constraint(self.terrains[2]))
        self.terrains[4].constraints.append(Constraint(self.terrains[3]))
        self.terrains[4].constraints.append(Constraint(self.terrains[4],6))
        self.terrains[4].constraints.append(Constraint(self.terrains[5]))
                
        self.terrains[5].constraints.append(Constraint(self.terrains[1],0))
        self.terrains[5].constraints.append(Constraint(self.terrains[2],2))
        self.terrains[5].constraints.append(Constraint(self.terrains[3],2))
        self.terrains[5].constraints.append(Constraint(self.terrains[4],2))
        self.terrains[5].constraints.append(Constraint(self.terrains[5],14))
        self.terrains[5].constraints.append(Constraint(self.terrains[6],1))
        
        self.terrains[6].constraints.append(Constraint(self.terrains[5],28))
        self.terrains[6].constraints.append(Constraint(self.terrains[6],32))
        self.terrains[6].constraints.append(Constraint(self.terrains[7],1))

        self.terrains[7].constraints.append(Constraint(self.terrains[6],9))
        self.terrains[7].constraints.append(Constraint(self.terrains[7]))

        row = 0
        while(row<self.rows):
            col = 0
            while(col<self.cols):
                self.tiles.append(MapTile(row,col,self.terrains))
                col+=1
            row+=1
                
    def draw_map(self):
        last_row = 0;
        row_text = ""
        for tile in self.tiles:
            if(tile.row != last_row):
                last_row = tile.row
                print(row_text)
                row_text = ""
            if(tile.collapsed):
                row_text += tile.terrain.code+tile.terrain.code
            else:
                row_text += str(len(tile.superpositions))+str(len(tile.superpositions))
        row_text += "\n"
        for terrain in self.terrains:
            row_text += terrain.code + ": " + terrain.name + " | "
        print(row_text)

    def setup_fixed_tiles(self): #configures water around edges and caldera in centre of island
        for tile in self.tiles:
            if(tile.row == 0 or tile.row == self.rows-1 or
                tile.col == 0 or tile.col == self.cols-1):

                tile.set_superpositions([self.terrains[0]])
                tile.collapsed = True
                tile.terrain = self.terrains[0]
            if(tile.row == int(self.rows/2) and tile.col == int(self.cols/2)):
                tile.set_superpositions([self.terrains[7]])
                tile.collapsed = True
                tile.terrain = self.terrains[7]

    def get_tile_by_coords(self,row,col):
        index = (row%self.rows)*self.cols+(col%self.cols)
        return self.tiles[index]

    def get_neighbours_of(self,tile):
        output = []
        output.append(self.get_tile_by_coords(tile.row-1,tile.col))
        output.append(self.get_tile_by_coords(tile.row+1,tile.col))
        output.append(self.get_tile_by_coords(tile.row,tile.col-1))
        output.append(self.get_tile_by_coords(tile.row,tile.col+1))
        return output

    def resolve_constraints(self):
        collapsed_count = 0
        row = 0
        while(row<self.rows):
            col = 0
            while(col<self.cols):
                tile = self.get_tile_by_coords(row,col)
                if(tile.collapsed):
                    collapsed_count+=1
                else:
                    self.constrain_tile(tile)
                    if(tile.collapsed): 
                        collapsed_count+=1
                col+=1
            row+=1
        return collapsed_count

    def constrain_tile(self,target_tile):
        check_list = self.get_neighbours_of(target_tile)
        cl_len = len(check_list)
        approvals = []
        tt_superpositions = len(target_tile.superpositions)
        ttsp = 0
        while(ttsp < tt_superpositions):
            approvals.append(0)
            check_index = 0
            while(check_index < cl_len):
                check_tile_superpos_count = len(check_list[check_index].superpositions)
                if(check_tile_superpos_count == len(self.terrains)):
                    approvals[ttsp]+=1
                else:
                    superpos_index = 0
                    while(superpos_index < check_tile_superpos_count):
                        constraint_count = len(check_list[check_index].superpositions[superpos_index].constraints)
                        constraint_index = 0
                        while(constraint_index<constraint_count):
                            if(target_tile.superpositions[ttsp] == check_list[check_index].superpositions[superpos_index].constraints[constraint_index].terrain):
                                approvals[ttsp]+=1
                                constraint_index=constraint_count
                                superpos_index = check_tile_superpos_count
                            constraint_index+=1
                        superpos_index +=1
                check_index +=1
            ttsp+=1
        permitted_superpositions = []
        ttsp = 0
        while(ttsp < tt_superpositions):
            if(approvals[ttsp] == cl_len):
                permitted_superpositions.append(target_tile.superpositions[ttsp])
            ttsp+=1
        target_tile.set_superpositions(permitted_superpositions)                
                
                
                

    def get_low_entropy_tiles(self):
        min_entropy = 10000000
        low_entropy_tiles = []
        row = 0
        while(row<self.rows):
            col = 0
            while(col<self.cols):
                tile = self.get_tile_by_coords(row,col)
                tile_superpos_count = len(tile.superpositions)
                if(tile_superpos_count>1):
                    if(tile_superpos_count<min_entropy):
                        min_entropy = tile_superpos_count
                        low_entropy_tiles = [tile]
                    elif(tile_superpos_count==min_entropy):
                        low_entropy_tiles.append(tile)
                col+=1
            row+=1
        return low_entropy_tiles

    def collapse_tile(self):
        low_entropy_tile_list = self.get_low_entropy_tiles()
        if(len(low_entropy_tile_list)>0):
            tile = low_entropy_tile_list[random.randint(1,len(low_entropy_tile_list))-1]
            tile_neighbours = self.get_neighbours_of(tile)
            possible_outcomes = dict()
            for terrain in tile.superpositions:
                possible_outcomes[terrain.name] = 0
            for ntile in tile_neighbours:
                if(ntile.collapsed):
                    for constraint in ntile.terrain.constraints:
                        try:
                            possible_outcomes[constraint.terrain.name] += constraint.weight
                        except:
                            pass #this terrain type is constrained by another neighbour
            choose_outcome_from = []
            for terrain , weight in possible_outcomes.items():
                i=0
                while (i<weight):
                    choose_outcome_from.append(terrain)
                    i+=1
            if(len(choose_outcome_from)>0):
                tile.set_superpositions([self.get_terrain_by_name(choose_outcome_from[random.randint(0,len(choose_outcome_from)-1)])])


    def get_terrain_by_name(self,name):
        for terrain in self.terrains:
            if terrain.name == name:
                return terrain

    def collapse_map(self):
        collapsed = 0
        prev_collapsed = 0
        collapsed_no_change_ticks = 0
        success = True
        while(collapsed < self.rows * self.cols and collapsed >= 0):
            prev_collapsed = collapsed
            collapsed = my_map.resolve_constraints()
            if(prev_collapsed == collapsed):
                collapsed_no_change_ticks +=1
            else:
                collapsed_no_change_ticks = 0
            self.collapse_tile()
            if(collapsed_no_change_ticks>50):
                success = False
                collapsed = -1
        return success       
build_count = 0
while(build_count<5):
    collapse_complete = False
    counter = 0
    while(not(collapse_complete)):
        my_map = GameMap(30,30)
        my_map.setup_fixed_tiles()
        collapse_complete = my_map.collapse_map()
        counter+=1
        if(counter>3):
            collapse_complete = True
            print("Collapse Failed repeatedly - Something is very wrong")
        #my_map.draw_map()
    my_map.draw_map()
    build_count += 1
