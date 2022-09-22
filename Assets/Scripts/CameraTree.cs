using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;


public class CameraTreeNode<T>
{
    private T root;
    private T parent;
    private List<CameraTreeNode<T>> children;

    public CameraTreeNode(T current_camera,T parent_camera)
    {
        root = current_camera;
        parent = parent_camera;
        children = new List<CameraTreeNode<T>>();
    }

    public T GetRoot()
    {
        return root;
    }

    public T GetParent()
    {
        return parent;
    }

    public List<CameraTreeNode<T>> GetChildren()
    {
        return children;
    }

    public void AddChildNode(CameraTreeNode<T> new_node)
    {
        children.Add(new_node);
    }

    public T FindInChild(T target_camera)
    {
        foreach (CameraTreeNode<T> camera_node in children)
        {
            if (Equals(camera_node.GetRoot(), target_camera))
                return target_camera;
        }
        return default(T);
    }

}

public class Program
{
    static bool Compare<T>(T x, T y) where T : class
    {
        return x == y;
    }
}


public class CameraTree<T>
{
    private List<CameraTreeNode<T>> camera_tree_list = new List<CameraTreeNode<T>>();

    /// <summary>
    /// Add new node into Tree
    /// </summary>
    /// <param name="child_camera"></param>
    public void AddChild(CameraTreeNode<T> new_node)
    {
        CameraTreeNode<T> new_root_node = new_node;
        foreach (CameraTreeNode<T> camera_sub_tree in camera_tree_list.ToList())
        {
            /// 1. it's someone's parent, could have multiple children
            if (Equals(camera_sub_tree.GetParent(), new_node.GetRoot()))
            {
                camera_tree_list.Remove(camera_sub_tree);
                new_node.AddChildNode(camera_sub_tree);
                continue;
            }

            /// 2. it's someone's child, can have only one parent
            CameraTreeNode <T> parent_node = FindParentNodeinChildren(camera_sub_tree, new_node);
            if (parent_node != null)
            {
                parent_node.AddChildNode(new_root_node);
                camera_tree_list.Remove(camera_sub_tree);
                new_root_node = camera_sub_tree;
                continue;
            }
        }

        ///3. it's not related to anyone Or put motified node into list
        camera_tree_list.Add(new_root_node);
    }



    /// <summary>
    /// traver the children to check if any child node is node's parent
    /// return certan child node, otherwise return null
    /// </summary>
    public CameraTreeNode<T> FindParentNodeinChildren(CameraTreeNode<T> tree_node,CameraTreeNode<T> new_node)
    {
        if (tree_node == null)
            return null;
        if (Equals(tree_node.GetRoot(), new_node.GetParent()))
            return tree_node;

        foreach(CameraTreeNode<T> child_node in tree_node.GetChildren())
        {
            CameraTreeNode<T> resullt_node = FindParentNodeinChildren(child_node, new_node);
            if (resullt_node != null)
                return resullt_node;
        }
        return null;
    }

    /// <summary>
    /// Prinit the tree
    /// </summary>
    public void PrintTree(List<CameraTreeNode<T>> camera_tree_list, CameraTreeNode<T> parent_node  = null)
    {
        foreach (CameraTreeNode<T> camera_tree_node in camera_tree_list)
        {

            if (parent_node != null)
                Debug.Log(camera_tree_node.GetRoot() + " with parent " + parent_node.GetRoot());
            else
                Debug.Log(camera_tree_node.GetRoot());
            PrintTree(camera_tree_node.GetChildren(), camera_tree_node);
        }
    }

    public List<CameraTreeNode<T>> GetTreeList()
    {
        return camera_tree_list;
    }
}
