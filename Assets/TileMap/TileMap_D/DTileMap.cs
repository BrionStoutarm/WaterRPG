using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.GameCenter;

public partial class DTileMap {
	
	int size_x;
	int size_y;
	
	int[,] map_data;
	
	List<DIsland> islands;

	/*
	 * 0 = deep ocean
	 * 1 = inland
	 * 2 = beach
	 * 3 = shallow ocean
	 */

	public DTileMap(int size_x, int size_y) {
		DIsland r;
		this.size_x = size_x;
		this.size_y = size_y;
		
		map_data = new int[size_x,size_y];
		
		for(int x=0;x<size_x;x++) {
			for(int y=0;y<size_y;y++) {
				map_data[x,y] = 0;
			}
		}
		
		islands = new List<DIsland>();
		
		int maxFails = 10;
		
		while(islands.Count < 10) {
			int r_sx = Random.Range(10, size_x - 10);
			int r_sy = Random.Range(10 ,size_y - 10);
			int r_sr = Random.Range(2, 5);
			r = new DIsland(r_sx, r_sy, r_sr);

			
			if(!IslandCollides(r)) {			
				islands.Add (r);
			}
			else {
				maxFails--;
				if(maxFails <=0)
					break;
			}
			
		}
		
		foreach(DIsland r2 in islands) {
			MakeIsland(r2);
		}
		//MakeShallows();
	}
	
	bool IslandCollides(DIsland r) {
		foreach(DIsland r2 in islands) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}

		//Make sure radius stays within bounds of map
		if (r.center_x + r.radius > size_x - 1 || r.center_x - r.radius < 0)
			return true;

		if (r.center_y + r.radius > size_y || r.center_y - r.radius < 0)
			return true;

		
		return false;
	}
	
	public int GetTileAt(int x, int y) {
		return map_data[x,y];
	}
	
	//TODO:
	// - different shapes and types depending on map biome
	void MakeIsland(DIsland r) {
		r.CreateIsland(map_data, r.center_x, r.center_y, DIsland.ENTRY_DIR.center, 0);
	}
	
	
	void MakeShallows() {
		for(int x=0; x< size_x;x++) {
			for(int y=0; y< size_y;y++) {
				if(map_data[x,y]==2) {
					MarkShallows(x, y);
				}
			}
		}
	}

	void MarkShallows(int x, int y) {
		if (map_data[x - 1, y] == 0)
			map_data[x - 1, y] = 3;

		if (map_data[x + 1, y] == 0)
			map_data[x + 1, y] = 3;

		if (map_data[x, y - 1] == 0)
			map_data[x, y - 1] = 3;

		if (map_data[x, y + 1] == 0)
			map_data[x, y + 1] = 3;

		if (map_data[x - 1, y - 1] == 0)
			map_data[x - 1, y - 1] = 3;

		if (map_data[x + 1, y + 1] == 0)
			map_data[x + 1, y + 1] = 3;

		if (map_data[x + 1, y - 1] == 0)
			map_data[x + 1, y - 1] = 3;

		if (map_data[x - 1, y + 1] == 0)
			map_data[x - 1, y + 1] = 3;
	}

}
