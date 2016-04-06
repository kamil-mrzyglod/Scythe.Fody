# Scythe.Fody
A code quality analyzer based on [Fody](https://github.com/Fody/Fody). It enables you to specify a number of characteristics of methods and classes, which will be checked after a build. If any of those is invalid, an error or a warning will be issued.

## Why?
Maintaining a codebase(especially when working with a team) is a difficult and painful task. We all know that feeling, that though we specified some rules and conventions, after few months they are all but just words.

Personally I found most code quality analyzers either cumbersome or outdated. They either require installing some additional plugins to VS or force you to use a command line to perform all needed task. I always wanted an easy tool, which I can embed in my build process and that's all - it just works.

`Scythe` works just like this - all you need to do is to download it from NuGet and ensure, that predefinied configuration satisfies you. Note that although it is created as a weaver, it doesn't change anything in your code(at least now) - instead it takes advantage of Fody's add-in model and analyzes code your compiler generated to ensure, that it satisfies your rules.
