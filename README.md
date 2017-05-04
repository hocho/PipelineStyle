**Pipeline Style**

Pipeline Style programming brings a more functional programming style of coding to C#. Inspired by the F# pipe operator and C# LINQ method syntax, this tiny set of C# extension functions, add more functional programming features to C#, using combinators.

Some of the benefits of Pipeline Style 
* Creates a fluent programming paradigm, converting statements to expressions and chaining them together
* Replaces code nesting with linear sequencing
* Eliminates variable declaration - does not even require a var
* Provides some form of variable immutability and scope isolation
* Structures code into small lambda expressions with clear responsibilities
* Builds sort of 'monadic computations'
Hopefully similar features make it into future versions of C# as first class parts of the language. Pipeline Style can be easily ported to and used in Java.

**Cheat sheet:** Pipeline Style uses extensions methods named with the verbs, 'Do' for Actions & 'To' for Functions. 'Do' extensions return the original parameter. 'To' extensions transform the original parameter to another value/type. 

See below a small sample of the extension methods and how they replace conventional C# coding.  

