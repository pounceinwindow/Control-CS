using System;
using System.ComponentModel;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class)]
class PluginnAttribute : Attribute
{
    public Type[] Dependencies;

    public PluginnAttribute(params Type[] depend)
    {
        this.Dependencies = depend;
    }
}
[Pluginn(typeof(PluginB), typeof(PluginC))]
class PluginA
{
    public PluginA()
    {


    }
}
[Pluginn(typeof(PluginD), typeof(PluginA))]
class PluginB
{
    public PluginB()
    {

    }
}
[Pluginn(typeof(PluginD), typeof(PluginB))]

class PluginC
{
    public PluginC()
    {

    }
}
[Pluginn(typeof(PluginC), typeof(PluginA))]

class PluginD
{
    public PluginD()
    {

    }
}

class Node
{
    string id { get; set; }
    public Node(string Id) { id = Id; }

    public static void Add() { }
}
class PluginnGraphBuilder
{
    public Dictionary<string, List<string>> BuildGraph()
    {
        Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
        var dep = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var e in dep) 
        {
            var attr = e.GetCustomAttribute<PluginnAttribute>();
            if (attr != null)
            {

                string pluginName = e.Name;
                var deps = attr.Dependencies.Select(t => t.Name).ToList();

                graph[pluginName] = deps;
            }
        }
        return graph;
    }

    public void PrintGraph(Dictionary<string, List<string>> graph)
    {
        foreach (var e in graph) { Console.WriteLine(($"{e.Key} -> {string.Join(", ", e.Value)}")); }
    }
}

class Program
{
    static void Main()
    {
        var builder = new PluginnGraphBuilder();
        var graph = builder.BuildGraph();
        builder.PrintGraph(graph);

    }
}
