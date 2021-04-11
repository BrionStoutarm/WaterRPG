using System.Numerics;
using System.Security.Permissions;
using System.Security.Policy;
using TMPro;
using UnityEditor;

public partial class DTileMap {
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
		private int x, y;
		private int r;
		//private int variance;

		public DIsland(int x_center, int y_center, int radius) {
			this.x = x_center;
			this.y = y_center;
			this.r = radius;
		}

		public int center_x {
			get {return x;}
		}
		
		public int center_y {
			get { return y; }
		}
		
		public int radius {
			get { return r; }
		}

		public bool CollidesWith(DIsland other) {
			Vector2 otherCenter = new Vector2(other.x, other.y);
			Vector2 center = new Vector2(x, y);

			float length = Vector2.Distance(otherCenter, center);

			if((int)length > other.radius && (int)length > radius)
				return true;

			return false;
		}

		public enum ENTRY_DIR {
			center = 0,
			up = 1,
			right = 2,
			down = 3,
			left = 4
		};

		public void CreateIsland(int [,] map_data, int x, int y, ENTRY_DIR entry_dir, int rad_iter) {
			if (rad_iter == radius) {
				map_data[x, y] = 2;
				return;
			}
			else if(entry_dir == ENTRY_DIR.center) {
				map_data[center_x, center_y] = 1;

				//go up
				CreateIsland(map_data, x, y - 1, ENTRY_DIR.up, rad_iter + 1);

				//go right
				CreateIsland(map_data, x + 1, y, ENTRY_DIR.right, rad_iter + 1);

				//go down
				CreateIsland(map_data, x, y + 1, ENTRY_DIR.down, rad_iter + 1);

				//go left
				CreateIsland(map_data, x - 1, y, ENTRY_DIR.left, rad_iter + 1);

			}
			//called from below
			else if(entry_dir == ENTRY_DIR.up) {
				map_data[x, y] = 1;

				//go up
				CreateIsland(map_data, x, y - 1, ENTRY_DIR.up, rad_iter + 1);

				//go right
				CreateIsland(map_data, x + 1, y, ENTRY_DIR.right, rad_iter + 1);

				//go left
				CreateIsland(map_data, x - 1, y, ENTRY_DIR.left, rad_iter + 1);
			}

			//called from right
			else if(entry_dir == ENTRY_DIR.right) {
				map_data[x, y] = 1;

				//go up
				CreateIsland(map_data, x, y - 1, ENTRY_DIR.up, rad_iter + 1);

				//go right
				CreateIsland(map_data, x + 1, y, ENTRY_DIR.right, rad_iter + 1);

				//go down
				CreateIsland(map_data, x, y + 1, ENTRY_DIR.down, rad_iter + 1);
			}

			//called from up
			else if(entry_dir == ENTRY_DIR.down) {
				map_data[x, y] = 1;

				//go right
				CreateIsland(map_data, x + 1, y, ENTRY_DIR.right, rad_iter + 1);

				//go down
				CreateIsland(map_data, x, y + 1, ENTRY_DIR.down, rad_iter + 1);

				//go left
				CreateIsland(map_data, x - 1, y, ENTRY_DIR.left, rad_iter + 1);
			}

			//called from the right
			else if(entry_dir == ENTRY_DIR.left) {
				map_data[x, y] = 1;

				//go up
				CreateIsland(map_data, x, y - 1, ENTRY_DIR.up, rad_iter + 1);

				//go down
				CreateIsland(map_data, x, y + 1, ENTRY_DIR.down, rad_iter + 1);

				//go left
				CreateIsland(map_data, x - 1, y, ENTRY_DIR.left, rad_iter + 1);
			}
		}

	}

}
