// See https://aka.ms/new-console-template for more information
using BRE;
using BRE.Entities.Sample;
using BRE.Rules;
using BRE.Rules.Sample;
using BRE.Syntaxes;
using BRE.Workflow;
using BRE.Workflow.Samples;
using BRE.Entities.Sample;
using BRE.Rules.Sample;
using Microsoft.CodeAnalysis.CSharp.Scripting;

WorkflowSample wf = new WorkflowSample();
Variables v = new Variables();
v.Add(new Variable { Name = "Text", Value = "Congratulation {0} the value is {1}." });
wf.Variables = v;
wf.Input = new GameEntity { Max = 100, Value = 66, Try = 50 };
bool b = wf.Start();
Console.WriteLine(b);

var left = new Operand("i.value");
var right = new Operand("i.max");
var condition = new LTOperator(left, right);
Console.WriteLine(condition.CSharp());
var lambda_cond_bloc = new Bloc();
lambda_cond_bloc.Operands.Add(new ReturnOperator(condition));
var lambda_cond = new LambdaExpression(new List<string> { "i", "v" }, lambda_cond_bloc);
Console.WriteLine(lambda_cond.CSharp());
var i_affect = new AffectationOperator(new Operand("int i"), new Operand(0));
var while_bloc = new Bloc();
var i_var = new Operand("i");
while_bloc.Operands.Add(new AffectationOperator(i_var, new AddOperator(i_var, new Operand(1))));
var while_inst = new WhileInstruction(new LTOperator(i_var, new Operand(10)), while_bloc);
Console.WriteLine(while_inst.CSharp());
var lambda_bloc = new Bloc();
lambda_bloc.Operands.Add(while_inst);
lambda_bloc.Operands.Add(new ReturnOperator(i_var));
var lambda = new LambdaExpression(new List<string> { "i", "o", "v" }, lambda_bloc);
Console.WriteLine(lambda.CSharp());


string code = @"using BRE.Entities.Sample;
public class Test1 {
    public book Condition1(GameEntity i, Variables v) {code1}
}";

string code1 = new Bloc(new List<Operand> { new ReturnOperator(condition) }).CSharp();
code = code.Replace("{code1}", code1);
Console.WriteLine(code);

var rule = new Rule<GameEntity, GameOutput> { Name = "test" };
var script = CSharpScript.EvaluateAsync(code);





// Test Eval (Payant ?)
/*Console.WriteLine("Hello, World!");
var r = new RoslynTest();
for (int i = 0; i < 100;i++) {
    Thread th = new Thread(r.Test);
    th.Start();
}*/

// Test Roslyn sans passer par Eval



Console.ReadLine();

