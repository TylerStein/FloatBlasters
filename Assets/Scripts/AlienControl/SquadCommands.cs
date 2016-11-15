using UnityEngine;
using System.Collections;

public enum SquadType { PERSUIT }

public class SquadCommander {
    //List of enemies in squad
    protected AlienControl[] squad;
    //Current squad target
    protected Transform target;
   
    protected SolarSystem system;

    //Constructor requires a list of squad units
    public SquadCommander(AlienControl[] inSquad, Transform target, SolarSystem sys){
        squad = inSquad;
        system = sys;
        this.target = target;
    }

    //Virtual function for inheriting classes to run commands
    virtual public void DoCommand() { /*Base class runs no commands*/ }

    /** Behaviors that can be commanded from DoCommand() **/
    //Attempt to reach and cut off a target
    protected void PersueTarget(Vector2 target)
    {
        //Iterate through each squad member
        for(int i = 0; i < squad.Length; ++i)
        {
            //Temporarily store the current iteration squad member for use
            AlienControl alien = squad[i];

            //Tell the alien to accelerate toward the target
            alien.seekTarget(target);
        }
    }

    //Attempt to line up with and fire at the target
    protected void AttackTarget(Vector2 target)
    {
        //Iterate through each squad member
        for (int i = 0; i < squad.Length; ++i)
        {
            squad[i].fireAt(target);
        }
    }

    /**Tools for squad behavior and descision**/
    //Find the averaged location of the squadmembers (middle)
    protected Vector2 getAvgLocation()
    {
        //Create 0 vector to add to
        Vector2 res = Vector2.zero;
        
        //Add each squad member's position to the result vector
        for(int i = 0; i < squad.Length; ++i)
        {
            res += squad[i].getPosition();
        }

        //Divide the result by the number of squad members to get average
        res /= squad.Length;

        //Return result
        return res;
    }
}

public class PersuitSquad : SquadCommander
{
    const int engageDist = 10;

    bool isRelocating;

    Transform mainTarget;
    Vector2 relocationTarget;

    //No extra actions needed in the constructor
    public PersuitSquad(AlienControl[] inSquad, Transform target, SolarSystem sys) : base(inSquad, target, sys) {
        isRelocating = false;
        mainTarget = target;
    }

    //Persuit squad finds the target and engages
    public override void DoCommand()
    {

        //Get the middle of the squad position
        Vector2 avg = getAvgLocation();

        Vector2 targetDir;
        Vector2 currentTarget;
        bool inRange = false;

        //Figure out the current target based on relocation status
        if (isRelocating)
        {
            currentTarget = relocationTarget;
        }
        else
        {
            currentTarget = new Vector2(mainTarget.position.x, mainTarget.position.y);
        }

        //Get the vector to the target
        targetDir = currentTarget - avg;

        //Get the distance to the target
        float distanceToTarget = targetDir.magnitude;

        //Check if in range, set bool
        if (distanceToTarget <= engageDist)
        {
            inRange = true;
        }

        //Switch status/act based on in range and location status
        if(inRange && isRelocating)
        {
            isRelocating = false;
        }
        else if(inRange && (!isRelocating))
        {
            isRelocating = true;
            AttackTarget(mainTarget.position);
            Vector2 finalRelocation = system.getRandomLocationNearTarget(avg, engageDist * 3, engageDist * 5);
            relocationTarget = finalRelocation;
        }

       
        //Persue the current target
        PersueTarget(currentTarget);
    }
}
