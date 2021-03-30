using UnityEngine;
using System.Collections.Generic;

public class DTileMap {
	
	/*protected class DTile {
		bool isWalkable = false;
		int tileGraphicId = 0;
		string name = "Unknown";
	}
	
	List<DTile> tileTypes;
	
	void InitTiles() {
		tileType[1].name = "Floor";
		tileType[1].isWalkable = true;
		tileType[1].tileGraphicId = 1;
		tileType[1].damagePerTurn = 0;
	}*/
	
	protected class DIsland {
		public int left;
		public int top;
		public int width;
		public int height;
		
		public bool isConnected=false;
		
		public int right {
			get {return left + width - 1;}
		}
		
		public int bottom {
			get { return top + height - 1; }
		}
		
		public int center_x {
			get { return left + width/2; }
		}
		
		public int center_y {
			get { return top + height/2; }
		}
		
		public bool CollidesWith(DIsland other) {
			if( left > other.right-1 )
				return false;
			
			if( top > other.bottom-1 )
				return false;
			
			if( right < other.left+1 )
				return false;
			
			if( bottom < other.top+1 )
				return false;
			
			return true;
		}
		
		
	}
	
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
			int rsx = Random.Range(4,14);
			int rsy = Random.Range(4,10);
			
			r = new DIsland();
			r.left = Random.Range(0, size_x - rsx);
			r.top = Random.Range(0, size_y-rsy);
			r.width = rsx;
			r.height = rsy;
			
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
		MakeShallows();
	}
	
	bool IslandCollides(DIsland r) {
		foreach(DIsland r2 in islands) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}
	
	public int GetTileAt(int x, int y) {
		return map_data[x,y];
	}
	
	//TODO:
	// - different shapes and types depending on map biome
	void MakeIsland(DIsland r) {
		
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				if(x==0 || x == r.width-1 || y==0 || y == r.height-1) {
					map_data[r.left+x,r.top+y] = 2;
				}
				else {
					map_data[r.left+x,r.top+y] = 1;
				}
			}
		}
		
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
