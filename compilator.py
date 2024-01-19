from game_sample import GameEntity, GameOutput
from rules import *
from syntax import *


@dataclass
class RuleTO(MetaRule):
    condition_json: str
    action_json: str

class Compilator:

    def __init__(self, variables: Variables):
        self.variables = variables
        self.python_fns: Dict[str, str] = {}
        self.parameters = ["i", "o", "v"]

    def compile(self, rules: List[RuleTO]):
        for r in rules:
            self.compile_ruleto(r)

    def adapters(self, rules: List[RuleTO]) -> List[Rule]:
        l = []
        for to in rules:
            l.append(self.adapter(to))
        return l

    def adapter(self, to: RuleTO) -> Rule:
        python = self.python_fns[f"{to.name}_action"]
        exec(python)
        r = Rule(to.id, to.index, to.name, to.priority, eval(self.python_fns[f"{to.name}_condition"]), eval(f"{to.name}_action"))
        return r



    def compile_ruleto(self, r: RuleTO):
        name = f"{r.name}_condition"
        res = self.compile_lambda_json(r.condition_json)
        self.python_fns[name] = res
        name = f"{r.name}_action"
        res = self.compile_def_json(name, r.action_json)
        self.python_fns[name] = res

    def compile_def_json(self, name:str, json: str):
        bloc: Bloc = jsonpickle.loads(json)
        if type(bloc.operands[-1]) is not ReturnOperator:
            bloc.operands[0] = ReturnOperator(bloc.operands[0])
        fn = Def(name, self.parameters, bloc)
        return fn.python()

    def compile_lambda_json(self, json: str):
        bloc = jsonpickle.loads(json)
        fn = Lambda(self.parameters, bloc)
        return fn.python()



if __name__ == '__main__':
    rules: List[RuleTO] = [
        RuleTO(id=0,
             index=0,
             name="success",
             priority=10,
             condition_json='{"py/object": "__main__.EqOperator", "value": null, "name": "==", "left": {"py/object": "__main__.Operand", "value": "i.value"}, "right": {"py/object": "__main__.Operand", "value": "o.value"}}',
             action_json='{"py/object": "__main__.Bloc", "operands": [{"py/object": "__main__.Operand", "value": "RuleStatus.SUCCESS"}]}'
             ),
        RuleTO(id=1,
               index=2,
               name="greater",
               priority=20,
               condition_json='{"py/object": "__main__.LTOperator", "value": null, "name": "<", "left": {"py/object": "__main__.Operand", "value": "o.value"}, "right": {"py/object": "__main__.Operand", "value": "i.value"}}',
               action_json='{"py/object": "__main__.Bloc", "operands": [{"py/object": "__main__.AffectationOperator", "value": null, "name": "=", "left": {"py/object": "__main__.Operand", "value": "next"}, "right": {"py/object": "__main__.TrueDivOperator", "value": null, "name": "//", "left": {"py/object": "__main__.AddOperator", "value": null, "name": "+", "left": {"py/object": "__main__.Operand", "value": "o.value"}, "right": {"py/object": "__main__.Operand", "value": "i.max"}}, "right": {"py/object": "__main__.Operand", "value": 2}}}, {"py/object": "__main__.IfInstruction", "value": null, "name": "if", "operand": {"py/object": "__main__.EqOperator", "value": null, "name": "==", "left": {"py/object": "__main__.Operand", "value": "o.value"}, "right": {"py/object": "__main__.Operand", "value": "next"}}, "bloc": {"py/object": "__main__.Bloc", "operands": [{"py/object": "__main__.AffectationOperator", "value": null, "name": "=", "left": {"py/object": "__main__.Operand", "value": "next"}, "right": {"py/object": "__main__.SubOperator", "value": null, "name": "-", "left": {"py/object": "__main__.Operand", "value": "next"}, "right": {"py/object": "__main__.Operand", "value": 1}}}]}, "condition": {"py/object": "__main__.EqOperator", "value": null, "name": "==", "left": {"py/object": "__main__.Operand", "value": "o.value"}, "right": {"py/object": "__main__.Operand", "value": "next"}}}, {"py/object": "__main__.AffectationOperator", "value": null, "name": "=", "left": {"py/object": "__main__.Operand", "value": "i.min"}, "right": {"py/object": "__main__.Operand", "value": "o.value"}}, {"py/object": "__main__.AffectationOperator", "value": null, "name": "=", "left": {"py/object": "__main__.Operand", "value": "o.value"}, "right": {"py/object": "__main__.Operand", "value": "next"}}, {"py/object": "__main__.AffectationOperator", "value": null, "name": "=", "left": {"py/object": "__main__.Operand", "value": "o.nb_try"}, "right": {"py/object": "__main__.AddOperator", "value": null, "name": "+", "left": {"py/object": "__main__.Operand", "value": "o.nb_try"}, "right": {"py/object": "__main__.Operand", "value": "1"}}}, {"py/object": "__main__.ReturnOperator", "value": null, "name": "return", "operand": {"py/object": "__main__.Operand", "value": true}}]}'
               ),
        ]
    c = Compilator(Variables([Variable("version", "0.1", "version")]))
    res = c.compile_lambda_json(rules[0].condition_json)
    print(res)
    res = c.compile_def_json("success_action", rules[0].action_json)
    print(res)
    c.compile_ruleto(rules[0])
    print(c.python_fns)
    c.compile(rules)
    print(c.python_fns)
    res = c.adapters(rules)
    print(res)
    input = GameEntity(0, 100, 33)
    output = GameOutput(33)
    print(res[0].condition(input, output, c.variables))
    status = res[0].action(input, output, c.variables)
    print(status)
    output = GameOutput(34)
    print(res[1].condition(input, output, c.variables))
    status = res[1].action(input, output, c.variables)
    print(status, output)



