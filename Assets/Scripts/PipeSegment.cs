using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class PipeSprites {
    public Sprite none;
    public Sprite up;
    public Sprite left;
    public Sprite down;
    public Sprite right;
    public Sprite upLeft;
    public Sprite leftDown;
    public Sprite downRight;
    public Sprite rightUp;
    public Sprite upDown;
    public Sprite leftRight;
    public Sprite exceptUp;
    public Sprite exceptLeft;
    public Sprite exceptDown;
    public Sprite exceptRight;
    public Sprite fourWay;

    public Sprite GetSprite(bool upConnected, bool downConnected, bool leftConnected, bool rightConnected)
    {
        int bitmask = (upConnected ? 1 : 0) | (downConnected ? 2 : 0) | (leftConnected ? 4 : 0) | (rightConnected ? 8 : 0);

        switch (bitmask)
        {
            case 0: return none;
            case 1: return up;
            case 2: return down;
            case 3: return upDown;
            case 4: return left;
            case 5: return upLeft;
            case 6: return leftDown;
            case 8: return right;
            case 9: return rightUp;
            case 10: return downRight;
            case 12: return leftRight;
            case 7: return exceptRight;
            case 11: return exceptLeft;
            case 13: return exceptDown;
            case 14: return exceptUp;
            case 15: return fourWay;
            default: return none;
        }
    }
}

public enum PipeConnection
{
    OtherPipe,
    Disconnected,
    Pull,
    Push
}

public class PipeSystem
{
    public List<PipeSegment> segments;
    public List<OutputInventory> inputs = new List<OutputInventory>(); // output from inventory into pipe system
    public List<InputInventory> outputs = new List<InputInventory>(); // input to inventory frome pipe system

    public PipeSystem(PipeSegment root)
    {
        List<PipeSegment> queue = new List<PipeSegment>() { root };
        segments = new List<PipeSegment>();

        AddSegment(root);

        while (queue.Count > 0)
        {
            PipeSegment current = queue[0];
            queue.RemoveAt(0);

            foreach (PipeSegment neighbor in current.PipeConnections())
            {
                if (!neighbor.BFSExplored)
                {
                    queue.Add(neighbor);
                    AddSegment(neighbor);
                }
            }
        }
        PipeManager.instance.systems.Add(this);
    }

    bool enabled = true;
    public void DisableSystem()
    {
        if (enabled == false) return;
        enabled = false;
        PipeManager.instance.systems.Remove(this);
    }

    public void Transfer()
    {
        List<Inventory> inputInventories = inputs.Select(inv => inv.GetOutputInventory()).ToList();
        List<Inventory> outputInventories = outputs.Select(inv => inv.GetInputInventory()).ToList();
        Inventory inInv = new Inventory(inputInventories);
        Inventory outInv = new Inventory(outputInventories);
        outInv.InsertItems(inInv.items, true);
    }

    public void AddSegment(PipeSegment segment)
    {
        segment.BFSExplored = true;
        segments.Add(segment);
        inputs.AddRange(segment.Inputs());
        outputs.AddRange(segment.Outputs());
        if (segment.system != null) segment.system.DisableSystem();
        segment.system = this;
    }

    public void RemarkAsUnexplored()
    {
        foreach (PipeSegment segment in segments)
        {
            segment.BFSExplored = false;
        }
    }

    public override string ToString()
    {
        return $"{inputs.Count} inputs, {outputs.Count} outputs, {segments.Count} segments";
    }
}
public class PipeSegment : TileObject
{
    PipeConnection upConnection = PipeConnection.Disconnected;
    PipeConnection downConnection = PipeConnection.Disconnected;
    PipeConnection leftConnection = PipeConnection.Disconnected;
    PipeConnection rightConnection = PipeConnection.Disconnected;

    TileObject upTile = null;
    TileObject downTile = null;
    TileObject leftTile = null;
    TileObject rightTile = null;

    [HideInInspector] public PipeSystem system = null;
    [HideInInspector] public bool BFSExplored = false;

    [SerializeField] GameObject extratorUp;
    [SerializeField] GameObject extratorDown;
    [SerializeField] GameObject extratorLeft;
    [SerializeField] GameObject extratorRight;

    [SerializeField] PipeSprites pipeSprites;
    public List<PipeSegment> PipeConnections()
    {
        List<PipeSegment> connections = new List<PipeSegment>();
        if (upConnection == PipeConnection.OtherPipe && upTile is PipeSegment upPipe) connections.Add(upPipe);
        if (downConnection == PipeConnection.OtherPipe && downTile is PipeSegment downPipe) connections.Add(downPipe);
        if (leftConnection == PipeConnection.OtherPipe && leftTile is PipeSegment leftPipe) connections.Add(leftPipe);
        if (rightConnection == PipeConnection.OtherPipe && rightTile is PipeSegment rightPipe) connections.Add(rightPipe);
        return connections;
    }
    public List<InputInventory> Outputs()
    {
        List<InputInventory> connections = new List<InputInventory>();
        if (upConnection == PipeConnection.Push && upTile is InputInventory upInv) connections.Add(upInv);
        if (downConnection == PipeConnection.Push && downTile is InputInventory downInv) connections.Add(downInv);
        if (leftConnection == PipeConnection.Push && leftTile is InputInventory leftInv) connections.Add(leftInv);
        if (rightConnection == PipeConnection.Push && rightTile is InputInventory rightInv) connections.Add(rightInv);
        return connections;
    }

    public List<OutputInventory> Inputs()
    {
        List<OutputInventory> connections = new List<OutputInventory>();
        if (upConnection == PipeConnection.Pull && upTile is OutputInventory upInv) connections.Add(upInv);
        if (downConnection == PipeConnection.Pull && downTile is OutputInventory downInv) connections.Add(downInv);
        if (leftConnection == PipeConnection.Pull && leftTile is OutputInventory leftInv) connections.Add(leftInv);
        if (rightConnection == PipeConnection.Pull && rightTile is OutputInventory rightInv) connections.Add(rightInv);
        return connections;
    }

    public void Awake()
    {
        UpdateSprite();
    }

    public void UpdateSystemNewConnection()
    {
        PipeSystem pipeSystem = new PipeSystem(this);
        pipeSystem.RemarkAsUnexplored();
    }

    public void UpdateSystemRemoveConnection()
    {
        List<PipeSystem> pipeSystems = new List<PipeSystem>();
        foreach (PipeSegment neighbor in PipeConnections())
        {
            if (neighbor.BFSExplored) continue;
            pipeSystems.Add(new PipeSystem(neighbor));
        }
        foreach (PipeSystem system in pipeSystems)
        {
            system.RemarkAsUnexplored();
        }
    }

    private void UpdateSprite()
    {
        bool upConnected = upConnection != PipeConnection.Disconnected && upTile != null;
        bool downConnected = downConnection != PipeConnection.Disconnected && downTile != null;
        bool leftConnected = leftConnection != PipeConnection.Disconnected && leftTile != null;
        bool rightConnected = rightConnection != PipeConnection.Disconnected && rightTile != null;
        spriteRenderer.sprite = pipeSprites.GetSprite(upConnected, downConnected, leftConnected, rightConnected);

        extratorUp.SetActive(upConnection == PipeConnection.Pull);
        extratorDown.SetActive(downConnection == PipeConnection.Pull);
        extratorLeft.SetActive(leftConnection == PipeConnection.Pull);
        extratorRight.SetActive(rightConnection == PipeConnection.Pull);
    }

    public void SetDirectionTile(Vector2 direction, TileObject neighbor)
    {
        if (direction == Vector2.up)
        {
            upTile = neighbor;
        }
        else if (direction == Vector2.down)
        {
            downTile = neighbor;
        }
        else if (direction == Vector2.left)
        {
            leftTile = neighbor;
        }
        else if (direction == Vector2.right)
        {
            rightTile = neighbor;
        }
        else
        {
            Debug.LogError($"{direction} is not a valid direction.");
        }
    }

    public TileObject GetDirectionTile(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return upTile;
        }
        else if (direction == Vector2.down)
        {
            return downTile;
        }
        else if (direction == Vector2.left)
        {
            return leftTile;
        }
        else if (direction == Vector2.right)
        {
            return rightTile;
        }
        else
        {
            Debug.LogError($"{direction} is not a valid direction.");
            return null;
        }
    }

    public void SetDirectionConnection(Vector2 direction, PipeConnection state)
    {
        if (direction == Vector2.up)
        {
            upConnection = state;
        }
        else if (direction == Vector2.down)
        {
            downConnection = state;
        }
        else if (direction == Vector2.left)
        {
            leftConnection = state;
        }
        else if (direction == Vector2.right)
        {
            rightConnection = state;
        }
        else
        {
            Debug.LogError($"{direction} is not a valid direction.");
        }
        UpdateSprite();
    }

    public PipeConnection GetDirectionConnection(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return upConnection;
        }
        else if (direction == Vector2.down)
        {
            return downConnection;
        }
        else if (direction == Vector2.left)
        {
            return leftConnection;
        }
        else if (direction == Vector2.right)
        {
            return rightConnection;
        }
        else
        {
            Debug.LogError($"{direction} is not a valid direction.");
            return PipeConnection.Disconnected;
        }
    }

    public override void TileUpdate(TileObject tile, Vector2 direction, bool neighborRemoved)
    {
        if (neighborRemoved)
        {
            SetDirectionTile(direction, null);
            SetDirectionConnection(direction, PipeConnection.Disconnected);
            return;
        }

        SetDirectionTile(direction, tile);

        if (tile is PipeSegment other)
        {
            SetDirectionConnection(direction, PipeConnection.OtherPipe); // temp for now
            return;
        }

        if (tile is InputInventory inInv)
        {
            SetDirectionConnection(direction, PipeConnection.Push);
        }
        else if (tile is OutputInventory outInv)
        {
            SetDirectionConnection(direction, PipeConnection.Pull);
        }
    }

    public override void Place(Vector2 position)
    {
        base.Place(position);
        UpdateSystemNewConnection();
    }

    public override void Remove()
    {
        base.Remove();
        upConnection = PipeConnection.Disconnected;
        downConnection = PipeConnection.Disconnected;
        leftConnection = PipeConnection.Disconnected;
        rightConnection = PipeConnection.Disconnected;
        UpdateSystemRemoveConnection();
    }

    [SerializeField] float clickDownEdgeBuffer = 0.2f;
    [SerializeField] float clickDragEdgeBuffer = 0.25f;
    public override void ClickDown(MouseInteractor mouse, bool firstClick) {
        if (!firstClick) return;
        float edgeBuffer = firstClick ? clickDownEdgeBuffer : clickDragEdgeBuffer;
        Edge edge = mouse.OnTileEdge(edgeBuffer);
        if (edge == Edge.None) return;
        else if (edge == Edge.Up) CycleEdge(Vector2.up);
        else if (edge == Edge.Down) CycleEdge(Vector2.down);
        else if (edge == Edge.Left) CycleEdge(Vector2.left);
        else if (edge == Edge.Right) CycleEdge(Vector2.right);
    }

    public void CycleEdge(Vector2 direction)
    {
        PipeConnection connection = GetDirectionConnection(direction);
        TileObject neighbor = GetDirectionTile(direction);
        if (neighbor is PipeSegment pipe)
        {
            PipeConnection newConnection;
            if (connection == PipeConnection.OtherPipe)
            {
                newConnection = PipeConnection.Disconnected;
            }
            else
            {
                newConnection = PipeConnection.OtherPipe;
            }
            SetDirectionConnection(direction, newConnection);
            pipe.SetDirectionConnection(-direction, newConnection);
        }
        else if (connection == PipeConnection.Disconnected)
        {
            if (neighbor is InputInventory)
            {
                SetDirectionConnection(direction, PipeConnection.Push);
            }
            else if (neighbor is OutputInventory)
            {
                SetDirectionConnection(direction, PipeConnection.Pull);
            }
        }
        else if (connection == PipeConnection.Push)
        {
            if (neighbor is OutputInventory)
            {
                SetDirectionConnection(direction, PipeConnection.Pull);
            }
            else
            {
                SetDirectionConnection(direction, PipeConnection.Disconnected);
            }
        }
        else
        {
            SetDirectionConnection(direction, PipeConnection.Disconnected);
        }

        if (GetDirectionConnection(direction) == PipeConnection.Disconnected)
        {
            UpdateSystemRemoveConnection();
        }
        else
        {
            UpdateSystemNewConnection();
        }
    }
}
