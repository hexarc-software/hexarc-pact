using System;
using System.Net.Http;
using Hexarc.Pact.Demo.Api.Models;
using Hexarc.Pact.Demo.Client;

var v1 = new Point { X = 0, Y = 0 };
var v2 = new Point { X = 0, Y = 1 };
var v3 = new Point { X = 1, Y = 0 };
var triangle = new Triangle { Vertex1 = v1, Vertex2 = v2, Vertex3 = v3 };

var client = new DemoClient(new HttpClient { BaseAddress = new Uri("http://hexarc-demo-api.herokuapp.com") });
var area = await client.Geometry.Area(triangle);

Console.WriteLine("The area of");
Console.WriteLine(ObjectDumper.Dump(triangle));
Console.WriteLine("is");
Console.WriteLine(area);

