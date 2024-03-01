from dataclasses import dataclass, field
from typing import Any, List, Dict
from dynamic_entities import Entity
from rules import Variables, Variable
import jsonpickle

def remove_empty_lines(s: str) -> str:
    if "\n\n" in s:
        return remove_empty_lines(s.replace("\n\n", "\n"))
    return s

class Token:

    def __init__(self):
        pass

class Operand(Token):

    def __init__(self, value: Any):
        super().__init__()
        self.value = value

    def python(self) -> str:
        return str(self.value)


class StringOperand:

    def __init__(self, value: str):
        self.value = value

    def python(self) -> str:
        return f'f"{self.value}"'


class DateTimeOperand(Operand):

    def __init__(self, value: str):
        super().__init__(value)

    def python(self) -> str:
        return f"datetime.fromisoformat('{self.value}')"


class Operator(Operand):

    def __init__(self, name: str):
        super().__init__(None)
        self.name = name


class UnaryOperator(Operator):

    def __init__(self, name: str, operand: Operand):
        super().__init__(name)
        self.operand = operand

    def python(self) -> str:
        o = self.operand.python()
        if " " in o:
            o = f"({o})"
        return f"{self.name} {o}"


class BinaryOperator(Operator):

    def __init__(self, name: str, left: Operand, right: Operand):
        super().__init__(name)
        self.left = left
        self.right = right

    def python(self) -> str:
        left = self.left.python()
        if " " in left:
            left = f"({left})"
        right = self.right.python()
        if " " in right:
            right = f"({right})"
        return f"{left} {self.name} {right}"

class TernaryOperator(Operator):

    def __init__(self, name: str, operand1: Operand, operand2: Operand, operand3: Operand):
        super().__init__(name)
        self.operand1 = operand1
        self.operand2 = operand2
        self.operand3 = operand3


class ManyOperator(Operator):

    def __init__(self, name: str, values: List[Operand]):
        super().__init__(name)
        self.values = values


class AndOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("and", left, right)


class OrOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("or", left, right)


class NotOperator(UnaryOperator):

    def __init__(self, right: Operand):
        super().__init__("not", right)


class EqOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name="==", left=left, right=right)


class NEOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("!=", left, right)


class LTOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name="<", left=left, right=right)

class GTOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name=">", left=left, right=right)


class LEOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name="<=", left=left, right=right)


class GEOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name=">=", left=left, right=right)


class AddOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("+", right, left)


class SubOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("-", right, left)


class MulOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("*", right, left)


class DivOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("/", right, left)


class TrueDivOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("//", right, left)


class PowOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("**", right, left)


class AffectationOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("=", left, right)

    def python(self):
        return f"{self.left.python()} = {self.right.python()}"


class ReturnOperator(UnaryOperator):

    def __init__(self, operand: Operand):
        super().__init__("return", operand)

    def python(self) -> str:
        return f"return {self.operand.python()}"

class RangeOperator(TernaryOperator):

    def __init__(self, operand1: Operand, operand2: Operand, operand3: Operand):
        super().__init__("range", operand1, operand2, operand3)

    def python(self):
        return f"range({self.operand1.python()}, {self.operand2.python()}, {self.operand3.python()})"


class Bloc(Token):

    def __init__(self, operands: List[Operand]=[], indent=2):
        super().__init__()
        self.operands = operands
        self.indent = indent

    def python(self):
        s = ""
        for o in self.operands:
            for _ in range(self.indent):
                s += "\t"
            s += f"{o.python()}\n"
        return s

class Instruction(Operand):

    def __init__(self, name: str, bloc: Bloc):
        super().__init__(None)
        self.name = name
        self.bloc = bloc

class UnaryInstruction(Instruction):

    def __init__(self, name: str, operand: Operand, bloc: Bloc):
        super().__init__(name, bloc)
        self.operand = operand

    def python(self):
        s = f"{self.name} {self.operand.python()}:\n"
        s += self.bloc.python()
        return remove_empty_lines(s)


class BinaryInstruction(Instruction):

    def __init__(self, name: str, left: Operand, right: Operand, bloc: Bloc):
        super().__init__(name, bloc)
        self.left = left
        self.right = right


class IfInstruction(UnaryInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__("if", condition, bloc)


class ElIfInstruction(UnaryInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__("elif", condition, bloc)


class ElseInstruction(UnaryInstruction):

    def __init__(self, bloc: Bloc):
        super().__init__("else", Operand(True), bloc)


class WhileInstruction(UnaryInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__("while", condition, bloc)

class ForInstruction(BinaryInstruction):

    def __init__(self, left: Operand, right: Operand, bloc: Bloc):
        super().__init__("for", left, right, bloc)

    def python(self):
        s = f"for {self.left.python()} in {self.right.python()}:\n"
        s += self.bloc.python()
        return remove_empty_lines(s)


class FunctionInstruction(UnaryInstruction):

    def __init__(self, name: str, parameters: List[str], bloc: Bloc):
        super().__init__(name, Operand(None), bloc)
        self.parameters = parameters

    def python(self) -> str:
        s = f"def {self.name}("
        s += ", ".join(self.parameters)
        s += "):\n"
        s += self.bloc.python()
        return remove_empty_lines(s)


class LambdaInstruction(UnaryInstruction):

    def __init__(self, parameters: List[str], operand: Operand):
        super().__init__("lambda", operand, Bloc())
        self.parameters = parameters

    def python(self) -> str:
        s = "lambda "
        s += ", ".join(self.parameters)
        s += f": {self.operand.python()}"
        return s


class Call(ManyOperator):

    def python(self) -> str:
        s = f"{self.name}("
        values = [v.python() for v in self.values]
        s += ", ".join(values)
        s +=")"
        return s

class MethodInstruction(FunctionInstruction):

    def __init__(self, name: str, parameters: List[str], bloc: Bloc):
        super().__init__(name, parameters, bloc)

    def python(self) -> str:
        s = f"\tdef {self.name}(self, "
        s += ", ".join(self.parameters)
        s += "):\n"
        s += self.bloc.python()
        return remove_empty_lines(s)

class ClassInstruction(Instruction):

    def __init__(self, name: str, attributes: List[str], methods: List[MethodInstruction], statics: Dict[str, Operand]={}, bloc: Bloc=Bloc([])):
        super().__init__(name, bloc)
        self.attributes = attributes
        self.methods = methods
        self.statics = statics

    def python(self):
        s = f"class {self.name}:\n"
        for static in self.statics:
            s += f"\t{self.name}.{static}={self.statics[static]}\n"
        s += "\tdef __init__(self"
        for attribute in self.attributes:
            s += f", {attribute}"
        s += "):\n"
        for attribute in self.attributes:
            s += f"\t\tself.{attribute} = {attribute}\n"
        s += self.bloc.python()
        s += "\n"
        for method in self.methods:
            s += f"{method.python()}\n"
        return s
class InstanciationInstruction(Call):

    def __init__(self, name: str, values: List[Operand]):
        super().__init__(name, values)

if __name__ == '__main__':
    left = Operand("i.value")
    right = Operand("o.value")
    op_equal = EqOperator(left, right)
    print(jsonpickle.dumps(Bloc([op_equal]), make_refs=False))
    print(op_equal)
    print(op_equal.python())
    params = ["i", "o", "v"]
    success = Operand("RuleStatus.SUCCESS")
    print(jsonpickle.dumps(Bloc([success]), make_refs=False))
    lambda_cond = LambdaInstruction(params, success)
    print(lambda_cond)
    print(lambda_cond.python())
    op_less = LTOperator(left, right)
    print(op_less.python())
    plus = AddOperator(right, Operand("i.max"))
    div2 = TrueDivOperator(plus, Operand(2))
    next = Operand("next")
    affect1 = AffectationOperator(next, div2)
    print(affect1.python())
    if_inst = IfInstruction(EqOperator(right, next),
                            Bloc([AffectationOperator(next, SubOperator(next, Operand(1)))], indent=1))
    print(if_inst.python())
    if_inst.bloc.indent+=3
    while_inst = WhileInstruction(
            AndOperator(EqOperator(right, next), NEOperator(right, left)),
            Bloc([if_inst,
                  ElIfInstruction(op_equal, Bloc([AffectationOperator(next, AddOperator(next, Operand(1)))], indent=5)),
                  ElseInstruction(Bloc([AffectationOperator(next, MulOperator(next, Operand(2)))], indent=4)),
                  AffectationOperator(next, SubOperator(next, Call("math.sin", [Operand(0)])))], indent=4))
    for_inst = ForInstruction(Operand("i"), RangeOperator(Operand(0), Operand(10), Operand(1)), Bloc([while_inst], indent=3))
    bloc = Bloc()
    bloc.operands.append(affect1)
    bloc.operands.append(for_inst)
    bloc.operands.append(AffectationOperator(Operand("i.min"), right))
    bloc.operands.append(AffectationOperator(right, next))
    bloc.operands.append(AffectationOperator(Operand("o.nb_try"), AddOperator(Operand("o.nb_try"), Operand("1"))))
    bloc.operands.append(ReturnOperator(Operand(True)))
    fn = MethodInstruction("action_less", params, bloc)
    class_ = ClassInstruction("RuleSet1", ["toto"], [fn])
    print(jsonpickle.dumps(class_, make_refs=False))
    print(class_.python())
    i = Entity()
    i.__dict__ = {"min": 0, "max": 100, "value": 33}
    entity = Entity()
    entity.__dict__ = {"value": (i.min + i.max) // 2, "nb_try": 0}
    v = Variables([Variable("version", "0.1", "version")])
    # print(jsonpickle.dumps(fn, indent="\t", make_refs=False))



