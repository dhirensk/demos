######  CLASS FACTORY PATTERN USING CLASSMETHODS
import sys
class Demo:
    c = None
    def __init__(self, a, b) -> None:
        self.a = a
        self.b = b
        
    @classmethod
    def setClassAttribute(cls, c_in):
        cls.c = c_in
    @classmethod
    def getClass(cls, a_in, b_in):
        DemoClass = cls(a_in, b_in)
        return DemoClass

    @classmethod
    def getClassAttribute(cls):
        return cls.c
    
    def getInstanceAttributes(self):
        return self.a, self.b
    
def class_factory_method(new_c_in):
    class DemoSubClass(Demo):
        c = new_c_in   
    return DemoSubClass
    
     
a_in = sys.argv[1]
b_in = sys.argv[2]
c_in = sys.argv[3]  

demo = Demo.getClass(a_in, b_in)
demo.setClassAttribute(c_in) 
a_out, b_out = demo.getInstanceAttributes()
print(a_out)  # 5
print(b_out)  # 6
print(demo.getClassAttribute())  # 7

demosubclass = class_factory_method(20)
print(demosubclass.getClassAttribute())  # 20
