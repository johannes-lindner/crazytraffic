using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GraphConfigParser
{
    public List<float> image_extent;
    public List<uint> image_shape;
    public string image_path;
    public List<float> rectangle_points;
    public float raster_size;
    public int graph_width;
    public int graph_heigth;
    public float rectangle_width;
    public float rectangle_height;
    public List<float> rectangle_center;

    //public GraphConfigParser() { 
    //    image_extent = new List<float>();
    //    image_shape = new List<uint>();
    //    rectangle_points = new List<float>();
    //    image_path = string.Empty;
    //    grid_size = 0;
    //}
    //
    //public GraphConfigParser(List<float> imageExtent, List<uint> imageShape, string imagePath, List<float> rectanglePoints, float gridSize)
    //{
    //    image_extent = imageExtent;
    //    image_shape = imageShape;
    //    image_path = imagePath;
    //    rectangle_points = rectanglePoints;
    //    grid_size = gridSize;
    //}
    //
    //public override string ToString()
    //{
    //    return $"Image Extent: {string.Join(", ", image_extent)}\n" +
    //           $"Image Shape: {string.Join(", ", image_shape)}\n" +
    //           $"Image Path: {image_path}\n" +
    //           $"Rectangle Points: {string.Join(", ", rectangle_points)}\n" +
    //           $"Grid Size: {grid_size}";
    //}
}

[System.Serializable]
public class GraphConfig
{
    public ImageExtent image_extent;
    public Vector3 image_shape;
    public string image_path;
    public List<Vector2> rectangle_points;
    public float raster_size;
    public Vector2 grid_size;
    public int rectangle_width;
    public int rectangle_height;
    public Vector2 rectangle_center;
    

    public GraphConfig(GraphConfigParser gcp) {
        image_extent = new ImageExtent(gcp.image_extent);
        image_shape = new Vector3(gcp.image_shape[0], gcp.image_shape[1], gcp.image_shape[2]);
        image_path = gcp.image_path;
        rectangle_points = GetRectanglePoints(gcp.rectangle_points);
        raster_size = gcp.raster_size;
        grid_size = new Vector2(gcp.graph_width, gcp.graph_heigth);
        rectangle_width = (int) gcp.rectangle_width;
        rectangle_height = (int) gcp.rectangle_height;
        rectangle_center = GetRectangleCenter(gcp.rectangle_center);
    }

    List<Vector2> GetRectanglePoints(List<float> l)
    {
        if (l.Count % 2 != 0) { 
            Debug.LogWarning("List with Rectangle Points has an unqueal Number!"); return null; }

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < l.Count; i+=2)
        {
            points.Add(new Vector2(l[i], l[i + 1]));
        }

        return points;
    }

    Vector2 GetRectangleCenter(List<float> l)
    {
        if (l.Count != 2) { 
            Debug.LogWarning("List Coordinates for Center Point is not equal 2!"); return new Vector2(0,0); }

        return new Vector2(l[0], l[1]);
    }
}

[System.Serializable]
public class ImageExtent
{
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;

    public ImageExtent(List<float> l)
    {
        if (l.Count==4)
        {
            xmin = l[0];
            xmax = l[1];
            ymin = l[2];
            ymax = l[3];
        }
        else
        {
            Debug.LogWarning("Image Extent could not be created! Number of Points not equal to 4.");
        }
    }
}