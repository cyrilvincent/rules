from dataclasses import dataclass, field
from typing import Any, List
from rules import Variables, Variable
import jsonpickle

def remove_empty_lines(s: str) -> str:
    if "\n\n" in s:
        return remove_empty_lines(s.replace("\n\n", "\n"))
    return s

class Operand:

    def __init__(self, value: Any):
        self.value = value

    def python(self) -> str:
        return str(self.value)


class StringValue(Operand):

    def __init__(self, value: str):
        super().__init__(value)

    def python(self) -> str:
        return f'"{self.value}"'


class DateTimeValue(Operand):

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
        o = {self.operand.python()}
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


class PlusOperator(BinaryOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("+", right, left)

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


class Bloc:

    def __init__(self, operands: List[Operand]=[]):
        self.operands = operands


class KeywordInstruction(UnaryOperator):

    def __init__(self, name: str, operand: Operand, bloc: Bloc):
        super().__init__(name, operand)
        self.bloc = bloc


class KeywordBinaryInstruction(BinaryOperator):

    def __init__(self, name: str, left: Operand, right: Operand, bloc: Bloc):
        super().__init__(name, left, right)
        self.bloc = bloc


class IfInstruction(KeywordInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__("if", condition, bloc)

    def python(self):
        s = f"{self.name} {self.operand.python()}:\n"
        for o in self.bloc.operands:
            s += f"\t{o.python().replace("\n\t", "\n\t\t")}\n"
        return remove_empty_lines(s)


class ElIfInstruction(IfInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__(condition, bloc)
        self.name = "elif"


class ElseInstruction(ElIfInstruction):

    def __init__(self, bloc: Bloc):
        super().__init__(Operand(True), bloc)



class WhileInstruction(IfInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__(condition, bloc)
        self.name = "while"

class ForInstruction(KeywordBinaryInstruction):

    def __init__(self, left: Operand, right: Operand, bloc: Bloc):
        super().__init__("for", left, right, bloc)

    def python(self):
        s = f"for {self.left.python()} in {self.right.python()}:\n"
        for o in self.bloc.operands:
            s += f"\t{o.python().replace("\n\t", "\n\t\t")}\n"
        return remove_empty_lines(s)


class Parameter:

    def __init__(self, name: str):
        self.name = name


class Def:

    def __init__(self, name: str, parameters: List[Parameter], bloc: Bloc):
        self.name = name
        self.parameters = parameters
        self.bloc = bloc

    def python(self) -> str:
        s = f"def {self.name}("
        s += ", ".join([p.name for p in self.parameters])
        s += "):\n"
        for o in self.bloc.operands:
            s += f"\t{o.python().replace("\n\t", "\n\t\t")}\n"
        return remove_empty_lines(s)


class Lambda(Operand):

    def __init__(self, parameters: List[Parameter], operand: Operand):
        super().__init__(None)
        self.parameters = parameters
        self.operand = operand

    def python(self) -> str:
        s = "lambda "
        s += ", ".join([p.name for p in self.parameters])
        s += f": {self.operand.python()}"
        return s


class FunctionCall(ManyOperator):

    def python(self) -> str:
        s = f"{self.name}("
        values = [v.python() for v in self.values]
        s += ", ".join(values)
        s +=")"
        return s


class Entity:

    def __init__(self):
        super().__init__()


if __name__ == '__main__':
    left = Operand("i.value")
    right = Operand("o.value")
    op_equal = EqOperator(left, right)
    print(jsonpickle.dumps(Bloc([op_equal]), make_refs=False))
    print(op_equal)
    print(op_equal.python())
    i_param = Parameter("i")
    o_param = Parameter("o")
    v_param = Parameter("v")
    params = [i_param, o_param, v_param]
    instruction = Operand("RuleStatus.SUCCESS")
    print(jsonpickle.dumps(Bloc([instruction]), make_refs=False))
    lambda_cond = Lambda(params, instruction)
    print(lambda_cond)
    print(lambda_cond.python())
    op_less = LTOperator(left, right)
    print(op_less.python())
    plus = PlusOperator(right, Operand("i.max"))
    div2 = TrueDivOperator(plus, Operand(2))
    next = Operand("next")
    affect1 = AffectationOperator(next, div2)
    print(affect1.python())
    if_inst = IfInstruction(EqOperator(right, next),
                            Bloc([AffectationOperator(next, SubOperator(next, Operand(1)))]))
    print(if_inst.python())
    while_inst = WhileInstruction(
            AndOperator(EqOperator(right, next), NEOperator(right, left)),
            Bloc([if_inst,
                  ElIfInstruction(op_equal, Bloc([AffectationOperator(next, AddOperator(next, Operand(1)))])),
                  ElseInstruction(Bloc([AffectationOperator(next, MulOperator(next, Operand(2)))])),
                  AffectationOperator(next, SubOperator(next, FunctionCall("math.sin", [Operand(0)])))]))
    for_inst = ForInstruction(Operand("i"), RangeOperator(Operand(0), Operand(10), Operand(1)), Bloc([while_inst]))
    bloc = Bloc()
    bloc.operands.append(affect1)
    bloc.operands.append(for_inst)
    bloc.operands.append(AffectationOperator(Operand("i.min"), right))
    bloc.operands.append(AffectationOperator(right, next))
    bloc.operands.append(AffectationOperator(Operand("o.nb_try"), PlusOperator(Operand("o.nb_try"), Operand("1"))))
    bloc.operands.append(ReturnOperator(Operand(True)))
    fn = Def("action_less", params, bloc)
    print(jsonpickle.dumps(fn, make_refs=False))
    print(fn.python())
    i = Entity()
    i.__dict__ = {"min": 0, "max": 100, "value": 33}
    entity = Entity()
    entity.__dict__ = {"value": (i.min + i.max) // 2, "nb_try": 0}
    v = Variables([Variable("version", "0.1", "version")])
    # print(jsonpickle.dumps(fn, indent="\t", make_refs=False))



