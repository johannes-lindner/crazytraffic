using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System.Xml.Linq;
using Newtonsoft.Json;

public class GraphSync : MonoBehaviour
{
    [SerializeField]
    string dataPath;

    [Header("Import")]
    public string import_fname = "myGraph.csv";
    public Dictionary<int,SerializableGraphNode> import_nodes = new Dictionary<int, SerializableGraphNode>();
    [SerializeField]
    int import_node_nr;
    public bool import_walkable = false;

    [Header("Export")]
    [Tooltip("Insert the Filename for your Graph Data here. \nIt will be safed in the Assets/Data directory")]
    public string export_fname = "myGraph.csv";
    
    [SerializeField]
    public List<SerializableGraphNode> nodes = new List<SerializableGraphNode>();

    public GraphConfig graphConfig;

    // Start is called before the first frame update
    void Start()
    {
        dataPath = System.IO.Path.Join(Application.dataPath, "Resources");
    }

    public void GetAllNodes()
    {
        var gg = AstarPath.active.data.gridGraph;

        gg.GetNodes(node => {            
            nodes.Add(new SerializableGraphNode(node));
        });
    }

    public void ImportGraph()
    {
        string fpath = System.IO.Path.Join(dataPath, import_fname);
        Debug.Log(fpath);
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (System.IO.StreamReader sr = new System.IO.StreamReader(fpath))
            {
                string line;
                uint c = 0;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if (c == 0) { 
                        c++; // Skip header line
                    } else {
                        string[] phrase = line.Split(',');
                        int NodeIndex = int.Parse(phrase[0]);
                        import_nodes.Add(NodeIndex, new SerializableGraphNode(line));
                    }
                }
                import_node_nr = import_nodes.Count;
            }
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.LogError("Import failed: " + e.Message);
        }
    }

    public void UpdateGraph()
    {
        var gg = AstarPath.active.data.gridGraph;

        gg.GetNodes(node => {
            int index = node.NodeIndex;
            SerializableGraphNode import_node = import_nodes[index];
            
            node.Penalty = import_node.Penalty;
            if (import_walkable) { node.Walkable =  import_node.Walkable; }
            // Find node with same ID in my list

        });

        // Recalculate all grid connections
        // This is required because we have updated the walkability of some nodes
        gg.GetNodes(node => gg.CalculateConnections((GridNodeBase)node));

        // If you are only updating one or a few nodes you may want to use
        // gg.CalculateConnectionsForCellAndNeighbours only on those nodes instead for performance.


        // Problem: NodeIndex hat nichts damit zu tun, wo sich der Node im Grid befindet, deshalb muss man den node immer suchen
        //var gg = AstarPath.active.data.gridGraph;
        //int x = 5;
        //int z = 8;
        //GridNodeBase node = gg.nodes[z * gg.width + x];

    }

    public void ExportGraph()
    {
        if(nodes.Count == 0)
        {
            GetAllNodes();
        }
        string fpath = System.IO.Path.Join(dataPath, export_fname);
        try
        {
            using (StreamWriter sw = new StreamWriter(fpath))
            {
                sw.WriteLine("nodeIndex,graphIndex,pos_x,pos_y,pos_z,penalty,area,destroyed,flags,tag,walkable");
                foreach (SerializableGraphNode node in nodes)
                {
                    sw.WriteLine(node.ToStringLine());
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Export failed: " + e.Message);
        }
    }

    public void ImportGraphConfig()
    {
        string fpath = System.IO.Path.Combine(dataPath, "0_graph_config.json");
        // Import JSON
        try
        {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(fpath))
            {
                string json = sr.ReadToEnd();
                GraphConfigParser graphConfigPar = JsonConvert.DeserializeObject<GraphConfigParser>(json);
                graphConfig = new GraphConfig(graphConfigPar);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Could not read json file: " + e.Message);
        }

        // Copy Background Image
        try
        {
            string fileToCopy = graphConfig.image_path;
            string destination = System.IO.Path.Join(Application.dataPath, "Resources", System.IO.Path.GetFileName(fileToCopy));
            File.Copy(fileToCopy, destination);
            Debug.Log("Copied File: " + destination);
        }catch(Exception e)
        {
            Debug.LogWarning("Failed Copying File: " + e.Message);
        }
    }

    public void CreateGraph()
    {
        ImportGraphConfig();


        // (1) Create Plane to project the image
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.name = "Background";
        plane.transform.position = new Vector3(graphConfig.rectangle_center.x, 0, graphConfig.rectangle_center.y);
        plane.transform.localScale = new Vector3(graphConfig.rectangle_width / 10f, 1, graphConfig.rectangle_height / 10f);

        //Find the Standard Shader
        Material BgMaterial = new Material(Shader.Find("Standard"));
        Texture2D imageAsset = Resources.Load("00_background") as Texture2D;
        Debug.Log(imageAsset.width);
        
        //Set Texture on the material
        var m_Renderer = plane.GetComponent<MeshRenderer>();
        //m_Renderer.material.EnableKeyword("_MainTex");
        BgMaterial.SetTexture("_MainTex", imageAsset);
        //Apply to GameObject
        plane.GetComponent<MeshRenderer>().material = BgMaterial;

        
        //AssetDatabase.CreateAsset(BgMaterial, "Assets/MyMaterial.mat");

        // (2) Create new Graph

        // This holds all graph data
        AstarData data = AstarPath.active.data;

        // This creates a Grid Graph
        GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;

        gg.center = new Vector3(graphConfig.rectangle_center.x, 0, graphConfig.rectangle_center.y);

        // Updates internal size from the above values
        gg.SetDimensions(graphConfig.rectangle_width, graphConfig.rectangle_height, graphConfig.grid_size);
        
        // Scans all graphs
        AstarPath.active.Scan();
    }
}

[System.Serializable]
public class SerializableGraphNode
{
    // To-do getters und setters
    public int NodeIndex;
    public uint GraphIndex;
    public Vector3 position;
    public uint Penalty;
    public uint Area;
    public bool Destroyed;
    public uint Flags;
    public uint Tag;
    public bool Walkable;

    public SerializableGraphNode(GraphNode node)
    {
        Area = node.Area;
        Destroyed = node.Destroyed;
        Flags = node.Flags;
        GraphIndex = node.GraphIndex;
        NodeIndex = node.NodeIndex;
        Penalty = node.Penalty;
        Tag = node.Tag;
        Walkable = node.Walkable;
        position = (Vector3) node.position;
    }

    public SerializableGraphNode(string line) 
    { // Create node from a Line in CSV File
        string[] phrase = line.Split(',');

        var CurrentCultureInfo = new CultureInfo("en", false);
        CurrentCultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        CurrentCultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        Thread.CurrentThread.CurrentUICulture = CurrentCultureInfo;
        Thread.CurrentThread.CurrentCulture = CurrentCultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = CurrentCultureInfo;

        if (phrase.Length == 11 )
        {
            NodeIndex = int.Parse(phrase[0]);
            GraphIndex = uint.Parse(phrase[1]);

            float pos_x = float.Parse(phrase[2],CurrentCultureInfo);
            float pos_y = float.Parse(phrase[3], CurrentCultureInfo);
            float pos_z = float.Parse(phrase[4], CurrentCultureInfo);
            position = new Vector3(pos_x, pos_y, pos_z);

            Penalty = uint.Parse(phrase[5]);
            Area = uint.Parse(phrase[6]);
            Destroyed = bool.Parse(phrase[7]);
            Flags = uint.Parse(phrase[8]);
            Tag = uint.Parse(phrase[9]);
            Walkable = bool.Parse(phrase[10]);
        }
    }

    public string ToStringLine()
    {
        return NodeIndex.ToString() + ", " + GraphIndex.ToString() + ", " + position.x.ToString().Replace(",", ".") + ", " + position.y.ToString().Replace(",", ".") + ", " + position.z.ToString().Replace(",", ".") + ", " + Penalty.ToString() + ", " + Area.ToString() + ", " + Destroyed.ToString() + ", " + Flags.ToString() + ", " + Tag.ToString() + ", " + Walkable.ToString();
    }
}
