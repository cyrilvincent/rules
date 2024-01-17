from dataclasses import dataclass, field
from typing import Any, List
from rules import Variables, Variable
import jsonpickle


@dataclass
class Operand:
    value: Any

    def python(self) -> str:
        return str(self.value)

class StringValue(Operand):

    def __init__(self, value: str):
        super().__init__(value)

    def python(self):
        return f'"{self.value}"'

class DateTimeValue(Operand):

    def __init__(self, value: str):
        super().__init__(value)

    def python(self):
        return f"datetime.fromisoformat('{self.value}')"

@dataclass
class ListValue(Operand):

    def __init__(self, value: List):
        super().__init__(value)

class Operator(Operand):

    def __init__(self, name: str):
        super().__init__(None)
        self.name = name


class UnaryOperator(Operator):

    def __init__(self, name: str, operand: Operand):
        super().__init__(name)
        self.operand = operand
    def python(self):
        o = {self.operand.python()}
        if " " in o:
            o = f"({o})"
        return f"{self.name} {o}"

class BinaryOperator(Operator):

    def __init__(self, name: str, left: Operand, right: Operand):
        super().__init__(name)
        self.left = left
        self.right = right

    def python(self):
        left = self.left.python()
        if " " in left:
            left = f"({left})"
        right = self.right.python()
        if " " in right:
            right = f"({right})"
        return f"{left} {self.name} {right}"

@dataclass
class ManyOperator(Operator):
    values: List[Operand]

@dataclass
class LogicalOperator(BinaryOperator):
    op_type: str = "logical"


@dataclass
class AndOperator(LogicalOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("and", left, right)

@dataclass
class OrOperator(LogicalOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("or", left, right)

@dataclass
class NotOperator(UnaryOperator):

    def __init__(self, right: Operand):
        super().__init__("not", right)

class ComparisonOperator(BinaryOperator):

    def __init__(self, name: str, left: Operand, right: Operand):
        super().__init__(name, left, right)



class EqualOperator(ComparisonOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name="==", left=left, right=right)

class NotEqualOperator(ComparisonOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("!=", left, right)

class LowerThanOperator(ComparisonOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__(name="<", left=left, right=right)

class ArithmeticOperator(BinaryOperator):

    def __init__(self, name: str, left: Operand, right: Operand):
        super().__init__(name, left, right)

class PlusOperator(ArithmeticOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("+", right, left)

class MinusOperator(ArithmeticOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("-", right, left)

class DivOperator(ArithmeticOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("/", right, left)

class IntegerDivOperator(ArithmeticOperator):

    def __init__(self, right: Operand, left: Operand):
        super().__init__("//", right, left)


class AffectationOperator(BinaryOperator):

    def __init__(self, left: Operand, right: Operand):
        super().__init__("=", left, right)

    def python(self):
        return f"{self.left.python()} = {self.right.python()}"

class ReturnOperator(UnaryOperator):

    def __init__(self, operand: Operand):
        super().__init__("return", operand)

    def python(self):
        return f"return {self.operand.python()}"

@dataclass
class Bloc:
    operands: List[Operand] = field(default_factory=list)

class KeywordInstruction(UnaryOperator):

    def __init__(self, name: str, operand: Operand, bloc: Bloc):
        super().__init__(name, operand)
        self.bloc = bloc

class IfInstruction(KeywordInstruction):

    def __init__(self, condition: Operand, bloc: Bloc):
        super().__init__("if", condition, bloc)
        self.condition = condition
        self.bloc = bloc

    def python(self):
        s = f"if {self.condition.python()}:\n"
        for o in self.bloc.operands:
            s += f"\t{o.python().replace("\n\t", "\n\t\t")}\n"
        return s


@dataclass
class Parameter:
    name: str

@dataclass
class Def:
    name: str
    parameters: List[Parameter]
    bloc: Bloc

    def python(self):
        s = f"def {self.name}("
        s += ", ".join([p.name for p in self.parameters])
        s += "):\n"
        for o in self.bloc.operands:
            s += f"\t{o.python().replace("\n\t", "\n\t\t")}\n"
        return s.replace("\n\n", "\n")

@dataclass
class Lambda:
    parameters: List[Parameter]
    operand: Operand

    def python(self):
        s = "lambda "
        s += ", ".join([p.name for p in self.parameters])
        s += f": {self.operand.python()}"
        return s

@dataclass
class FunctionCall(ManyOperator):

    def python(self):
        s = f"{self.name}("
        values = [v.python() for v in self.values]
        s += ", ".join(values)
        s +=")"

class Entity:

    def __init__(self):
        super().__init__()


if __name__ == '__main__':
    left = Operand("i.value")
    right = Operand("o.value")
    op_equal = EqualOperator(left, right)
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
    op_less = LowerThanOperator(left, right)
    print(op_less.python())
    plus = PlusOperator(right, Operand("i.max"))
    div2 = IntegerDivOperator(plus, Operand(2))
    next = Operand("next")
    affect1 = AffectationOperator(next, div2)
    print(affect1.python())
    if_inst = IfInstruction(EqualOperator(right, next),
                            Bloc([AffectationOperator(next, MinusOperator(next, Operand(1)))]))
    print(if_inst.python())
    bloc = Bloc([])
    bloc.operands.append(affect1)
    bloc.operands.append(if_inst)
    bloc.operands.append(AffectationOperator(Operand("i.min"), right))
    bloc.operands.append(AffectationOperator(right, next))
    bloc.operands.append(AffectationOperator(Operand("o.nb_try"), PlusOperator(Operand("o.nb_try"), Operand("1"))))
    bloc.operands.append(ReturnOperator(Operand(True)))
    fn = Def("action_less", params, bloc)
    print(jsonpickle.dumps(fn, make_refs=False))
    print(fn.python())
    i = Entity()
    i.__dict__ = {"min": 0, "max": 100, "value": 33}
    o = Entity()
    o.__dict__ = {"value": (i.min + i.max) // 2, "nb_try": 0}
    v = Variables([Variable("version", "0.1", "version")])
    print(jsonpickle.dumps(fn, indent="\t", make_refs=False))















