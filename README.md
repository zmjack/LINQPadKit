# LINQPadKit
Extension Toolkit for LINQPad.

<br/>

## Prism

```csharp
void Main()
{
    Prism.Import();    
    new Prism("csharp")
    {
"""
Console.WriteLine("Render code in LINQPad !");
"""
    }.Dump();
}
```

```csharp
Console.WriteLine("Render code in LINQPad !");
```

<br/>

## Mermaid

```csharp
void Main()
{
    Mermaid.Import();    
    new Mermaid
    {
"""
graph TB
A --> B
A --> C
"""
    }.Dump("Mermaid");
}
```

```mermaid
graph TB
A --> B
A --> C
```

### TreeGraph

```csharp
void Main()
{
    Mermaid.Import();
    new Mermaid.TreeGraph
    {
        new TreeNode("A")
        {
            new TreeNode("B")
            {
                new TreeNode("D"),
                null,
            },
            new TreeNode("C")
            {
                null,
                new TreeNode("E"),
            },
        }
    }.Dump("Mermaid.TreeGraph");
}
```

```mermaid
graph TB
A((A)) --- B((B))
B((B)) --- D((D))
B((B)) --- 8db619b9-315e-44aa-b101-75aeba2ceda2(( ))
style 8db619b9-315e-44aa-b101-75aeba2ceda2 fill:transparent,stroke-width:0px
A((A)) --- C((C))
C((C)) --- 063a88a0-0be4-4d66-a5cb-7f3924fa682a(( ))
style 063a88a0-0be4-4d66-a5cb-7f3924fa682a fill:transparent,stroke-width:0px
C((C)) --- E((E))
linkStyle 2 stroke-width:0px
linkStyle 4 stroke-width:0px
```



