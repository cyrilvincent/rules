// See https://aka.ms/new-console-template for more information
using BRE;
using BRE.Entities.Sample;
using BRE.Rules;
using BRE.Workflow;
using BRE.Workflow.Samples;

WorkflowSample wf = new WorkflowSample();
Variables v = new Variables();
v.Add(new Variable { Name = "Text", Value = "Congratulation {0} the value is {1}." });
wf.Variables = v;
wf.Input = new GameEntity { Max = 100, Value = 66, Try = 50 };
bool b = wf.Start();
Console.WriteLine(b);

// Test Eval (Payant ?)
/*Console.WriteLine("Hello, World!");
var r = new RoslynTest();
for (int i = 0; i < 100;i++) {
    Thread th = new Thread(r.Test);
    th.Start();
}*/

// Test Roslyn sans passer par Eval
// Test Dynamic Linq
// Test Expression


Console.ReadLine();

