from dataclasses import dataclass, field
from typing import Any, List


@dataclass
class BaseEntity:
    name: str

class Class:
    pass

@dataclass
class Parameter(BaseEntity):
    type: Class

@dataclass
class Attribute(Parameter):
    value: Any

@dataclass
class AttributeList(Parameter):
    value: List[Any]

@dataclass
class Method(BaseEntity):
    type: Class
    parameters: List[Parameter]
    python: str
    json: str

@dataclass
class Class(BaseEntity):
    attributes: List[Attribute | AttributeList] = field(default_factory=list)
    methods: List[Method] = field(default_factory=list)
    python_type: Any = object

class Instance(Class):

    def __init__(self, name: str, attributes: List[Attribute | AttributeList], class_: Class):
        super().__init__(name, attributes, [])
        self.class_ = class_

class Entity:

    def __init__(self):
        super().__init__()

def class_factory(c: Class):
    return type(c.name, (Entity,), {} )

def instance(class_: Any, i: Instance):
    instance: Entity = class_()
    dict = {}
    for a in i.attributes:
        dict[a.name] = a.value
    for m in i.class_.methods:
        if m.python is not None:
            s = f"def {m.name}:\n"
            for row in m.python.split("\n"):
                s += f"\t{row}\n"
            exec(s)
            dict[m.name] = eval(m.name)
    instance.__dict__ = dict
    return instance


int_type = Class("int", [], [], int)
float_type = Class("float", [], [], float)
string_type = Class("string", [], [], str)

if __name__ == '__main__':
    input_class = Class("GameInput", [Attribute("min", int_type, None),  Attribute("max", int_type, None),  Attribute("value", int_type, None)], [])
    input_instance = Instance("gi1", [Attribute("min", int_type, 0),  Attribute("max", int_type, 100),  Attribute("value", int_type, 33)], input_class)
    output_class = Class("GameOutput", [Attribute("value", int_type, None),  Attribute("nb_try", int_type, None)], [])
    output_instance =  Instance("go1", [Attribute("value", int_type, 0),  Attribute("nb_try", int_type, 0)], output_class)
    c = class_factory(input_class)
    print(c)
    input = instance(c, input_instance)
    print(input)
    print(input.value)
    output = instance(class_factory(output_class), output_instance)
    print(output.nb_try)
    x_class = Class("x", [Attribute("input", input_class, None), AttributeList("output", output_class, [])], [])
    x_instance = Instance("x1", [Attribute("input", input_class, input), AttributeList("output", output_class, [output, output])], x_class)
    c = class_factory(x_class)
    print(c)
    x = instance(c, x_instance)
    print(x)
    print(x.input.value)
    print(x.output)
    print(x.output[0].value)









