using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour {
    private Animator anim;
    private GameMaster board;
    public int pendingX, pendingY, critterID, touchID, x, y, state = 0;
    public float disX, disY;
    public string Name, SkullColor, AuraColor;
    public bool Draggable;
    private bool justReleased = false;
    private bool happy, shock = false;

    //THis is used when evaluating chain Zenblocks. Given a list of cardinally adjacent
    //neighbors with matching colors, we get the highest state among them.
    //state 3 = win condition, state 4 overrides that. State 2 = movement. State 1 is default.
    //since we take the highest state, it prioritises state 4 (somethings wrong to make a zen unhappy)
    //over the zen being happy (state 3).
    private int EmpathyCheck(List<CharacterData> friends)
    {
        int state = 1;

        for (int a = 0; a < friends.Count; a++)
        {
            if (state == 4)
                break;

            if (friends[a].GetState() > state && friends[a].state != 2)
                state = friends[a].GetState();
        }

        if (state == 4)
        {
            shock = true;
            return 4;
        }
        
        return state;
    }

    //Utility Method, Given a coordinates, get a key that the positional dictionaries are mapped to.
    public string GenKey(int x, int y)
    {
        return "" + (this.x + x) + "." + (this.y + y);
    }

    public int GetState()
    {
        return state;
    }


    public bool IsHappy()
    {
        shock = false;

        if (board.StandingAtopEvil(x, y)) //we freak out when we stand on top of evil tiles.
        {
            shock = true;
            return happy = false;
        }

        if (this.state != 2)//state == 2 if zen is being dragged by finger
        {
            if (this.AuraColor.Equals("2"))
            {
                List<CharacterData> friends = board.GetSameSkulls(x, y, SkullColor);

                if (friends.Count > 0)
                {
                    int state = EmpathyCheck(friends);

                    if (state == 4)
                        return happy = false;
                    if (state == 3)
                        return happy = true;
                }
            }

         

            if (board.ContainsZenHouse(x, y))  //if we're standing atop a house tile.
                if (board.GetZenHouse(x, y).IsFriendlyHouse(SkullColor))
                    return happy = true;
                else
                {
                    shock = true;
                    return happy = false;
                }

            if (this.AuraColor.Equals("3")) //if it's a red friendship zen (levels 80+ typically).
            {                               //basically theyre happy if theyre next to eachother and identical.
                List<CharacterData> friends = board.GetSameSkulls(x, y, SkullColor);

                if (friends.Count > 0)
                    for(int a = 0; a < friends.Count; a++)
                        if (friends[a].AuraColor.Equals(this.AuraColor))
                            if(friends[a].GetState() == 1 || friends[a].GetState() == 3)
                                return happy = true;
                            else if(friends[a].GetState() == 4)
                            {
                                shock = true;
                                return happy = false;
                            }
            }
        }

        return happy = false;
    }

    //given a ZenCritter as a neighbor, do they share the same color?
    //if yes, they're friends. Otherwise naw dawg.
    public bool IsFriends(CharacterData neighbor)
    {
        if (neighbor == null)
            return false;

        if (this.AuraColor.Equals("Rainbow") || neighbor.AuraColor.Equals("Rainbow"))
            return true;
        if (this.AuraColor.Equals(neighbor.AuraColor))
            return true;

        return false;
    }

    public void SetAura(int a)
    {

        AuraColor = "" + a;

        if (AuraColor.Equals("1"))
            this.Draggable = false;

        foreach (Transform child in transform)
        {
            if(child.tag == "Aura")
            {
                child.GetComponent<AuraData>().SetAura(a);
            }
        }

    }

    //useful for tracking moves and fetching a critter by his id...O(1) hashtable lookups
    //when we map to the ID.
    public int GetID()
    {
        return critterID;
    }

    public void SetID(int id)
    {
        this.critterID = id;
    }

    public void SetTouch(int id)
    {
        if (!Draggable) //if its not draggable (black aura, we ignore the touch)
            return;

        touchID = id;
    }

    public int GetTouch()
    {
        return touchID;
    }

    //only called when a zencritter is set down...efficiency
    public void clearTouch()
    {
        touchID = -1;
        justReleased = true;
    }

    //returns whether or not there critter is being touched atm
    public bool isTouched()
    {
        return touchID != -1;
    }

    public void SetInitialPosition(Vector2 p) //Called only when we instantiate the gameobject
    {
        x = (int)p.x;
        y = (int)p.y;

        pendingX = x;
        pendingY = y;
        disX = x;
        disY = y;
        transform.position = new Vector3(disX, disY, 0);
    }

    public void SetPosition(Vector2 p)
    {
        if (!Draggable) //gotta make sure some blocks arent processed via touch
            return;

        pendingX = (int)p.x;
        pendingY = (int)p.y;
    }

    public void ResetPendingPosition()
    {
        pendingX = x;
        pendingY = y;
    }

	// Use this for initialization
	void Start () {
        touchID = -1;
        anim = GetComponent<Animator>();
        
        x = (int) transform.position.x;
        y = (int) transform.position.y;

        pendingX = x;
        pendingY = y;
	}

    void Update()
    {
        if (board == null)
            board = Board();
        
        if((disX != x) || (disY != y) || (pendingX != x) || (pendingY != y))
            UpdatePosition();
        
        SetState(); //changes animation according to the state.
                    //state is determined in IsHappy() method.
    }

    //Move Methods update movement position.
    public void MoveDown(bool pushed)
    {
        board.ClearCritterPosition(x, y);
        y--;
        board.SetCritterPosition(x, y, this);

        if (!pushed)
        {
            board.DecrementGhost(x, y + 2);
            board.DecrementGhost(x + 1, y + 2);
        }
        
    }
    public void MoveUp(bool pushed)
    {
        board.ClearCritterPosition(x, y);
        y++;
        board.SetCritterPosition(x, y, this);

        if (!pushed)
        {
            board.DecrementGhost(x, y - 1);
            board.DecrementGhost(x + 1, y - 1);
        }
        
    }
    public void MoveLeft(bool pushed)
    {
        board.ClearCritterPosition(x, y);
        x--;
        board.SetCritterPosition(x, y, this);

        if (!pushed) {
            board.DecrementGhost(x + 2, y);
            board.DecrementGhost(x + 2, y + 1);
        }
        
    }

    public void MoveRight(bool pushed)
    {
        board.ClearCritterPosition(x, y);
        x++;
        board.SetCritterPosition(x, y, this);

        if (!pushed)
        {
            board.DecrementGhost(x - 1, y);
            board.DecrementGhost(x - 1, y + 1);
        }
        
    }
    void UpdatePosition() //handles updating ZenCritter positions using pendingX&Y
    {                     //modified in setPosition(), specifically in the TouchHandler      
        int deltaX = pendingX - x;
        int deltaY = pendingY - y;
        bool moved = false;

        if(Mathf.Abs(deltaX) >= Mathf.Abs(deltaY))
        {
            if (deltaX > 1)
            {
                if (CanMove(2, 0))
                {
                    MoveRight(false);
                    moved = true;
                }
            }
            else if (deltaX < 0)
            {
                if (CanMove(4, 0))
                {
                    MoveLeft(false);
                    moved = true;
                }
            }
        }else
        {
            if (deltaY > 1)
            {
                if (CanMove(1, 0))
                {
                    MoveUp(false);
                    moved = true;
                }
            }
            else if (deltaY < 0)
            {
                if (CanMove(3, 0))
                {
                    MoveDown(false);
                    moved = true;
                }
            }
        }
        if(!moved)
        {
            if (Mathf.Abs(deltaX) < Mathf.Abs(deltaY))
            {
                if (deltaX > 1)
                {
                    if (CanMove(2, 0))
                    {
                        MoveRight(false);
                        moved = true;
                    }
                }
                else if (deltaX < 0)
                {
                    if (CanMove(4, 0))
                    {
                        MoveLeft(false);
                        moved = true;
                    }
                }
            }
            else
            {
                if (deltaY > 1)
                {
                    if (CanMove(1, 0))
                    {
                        MoveUp(false);
                        moved = true;
                    }
                }
                else if (deltaY < 0)
                {
                    if (CanMove(3, 0))
                    {
                        MoveDown(false);
                        moved = true;
                    }
                }
            }
        }

        UpdateVisuals();

        if(justReleased)
        {
                board.ItsTimeToCheckHappy();
                justReleased = false;
        }
    }

    private void UpdateVisuals()
    {
        float speed = .67f; //higher speed is faster transition.

        disX = x + ((transform.position.x - x) * speed);
        disY = y + ((transform.position.y - y) * speed);
        float displacement = Mathf.Abs(x - transform.position.x)* .1f + Mathf.Abs(y - transform.position.y) * .1f;
        
        if (Mathf.Abs(disX - x) < .1f)
            this.disX = x;
        if (Mathf.Abs(disY - y) < .1f)
            this.disY = y;
                
        transform.position = new Vector3(this.disX, this.disY, transform.position.z);
    }

    private GameMaster Board()
    {
        if(GameObject.Find("GameMaster") != null)
            return GameObject.Find("GameMaster").GetComponent<GameMaster>();

        return null;
    }

    //given a direction (1234->NESW), can it move.
    //this recurses inward by 1, cause you can push 1 zencritter, but it cant push a third
    bool CanMove(int direction, int isPushed)
    {
        if(direction == 1)
        {
            if (board.ContainsTile(x, y + 2) && board.ContainsTile(x + 1, y + 2))
            { //needs to contain the tiles to begin with
                if (!board.ContainsCritter(x - 1, y + 2) && !board.ContainsCritter(x + 1, y + 2))
                { //needs to not have diagonals occupied
                    if (!board.ContainsCritter(x, y + 2))
                    { //if no critters in the way, lifes simple.
                        return true;
                    }
                    else if (isPushed == 0)
                    { //otherwise, if this is the first time we pushing
                        CharacterData d = board.GetCritter(x , y + 2);

                        if (d.touchID == -1 && d.CanMove(1, 1))
                        { //if we're not touching the blocking critter AND it too can move

                            d.MoveUp(true);
                            d.UpdateVisuals();
                            d.ResetPendingPosition();
                            return true; //gorgeous.
                        }
                    }
                }

                pendingY = y;
            }
        }
        if(direction == 2)
        {
            if (board.ContainsTile(x + 2, y) && board.ContainsTile(x + 2, y + 1))
            {
                if (!board.ContainsCritter(x + 2, y - 1) && !board.ContainsCritter(x + 2, y + 1))
                {
                    if (!board.ContainsCritter(x + 2, y))
                    {
                        return true;
                    }
                    else if (isPushed == 0)
                    {
                        CharacterData d = board.GetCritter(x + 2, y);

                        if (d.touchID == -1 && d.CanMove(2, 1))
                        {

                            d.MoveRight(true);
                            d.UpdateVisuals();
                            d.ResetPendingPosition();
                            return true;
                        }
                    }
                }
                pendingX = x;
            }
        }
        if(direction == 3)
        {
            if (board.ContainsTile(x, y - 1) && board.ContainsTile(x + 1, y - 1))
            {
                if (!board.ContainsCritter(x - 1, y - 2) && !board.ContainsCritter(x + 1, y - 2))
                {
                    if (!board.ContainsCritter(x, y - 2))
                    {
                        return true;
                    } else if(isPushed == 0)
                    {
                        CharacterData d = board.GetCritter(x, y - 2);

                        if(d.touchID == -1 && d.CanMove(3, 1))
                        {
                            d.MoveDown(true);
                            d.UpdateVisuals();
                            d.ResetPendingPosition();
                            return true;
                        }
                    }
                }
                pendingY = y;
            }
        }
        if (direction == 4)
        {
            if (board.ContainsTile(x - 1, y) && board.ContainsTile(x - 1, y + 1))
            {
                if (!board.ContainsCritter(x - 2, y - 1) && !board.ContainsCritter(x - 2, y + 1))
                {
                    if (!board.ContainsCritter(x - 2, y))
                    {
                        return true;
                    }
                    else if (isPushed == 0)
                    {
                        CharacterData d = board.GetCritter(x - 2, y);

                        if (d.touchID == -1 && d.CanMove(4, 1))
                        {
                            d.MoveLeft(true);
                            d.UpdateVisuals();
                            d.ResetPendingPosition();
                            return true;
                        }
                    }
                }

                pendingX = x;
            }
        }
        return false;
    }

    //changes the state to switch animations.
    public void SetState()
    {
        if (touchID != -1)
        {
            if (state == 2)
                return;
            state = 2;
        }
        else if(shock)
        {
            if (state == 4)
                return;

            state = 4;
        }
        else if(happy)
        {
          
            if (state == 3)
                return;

            state = 3;
        }else {
            if (state == 1)
                return;
            state = 1;
        }

        anim.SetInteger("state", state);
        
    }

}
