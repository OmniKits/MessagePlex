# MessagePlex

MessagePlex is a library for in-proc message broadcasting via simple `IEnumerable<T>` model.

## Why start MessagePlex?

When I began to design a prototype of this back in 2011, I had no knowledge about Reactive Extensions,
 all I wanted is to build a in-proc message broadcasting component which can take advantage from .NET GC.

As days go by, I've refactored it and changed its name several times, by then I eventually learned about Rx.
 And for now I'm 99% sure that Rx can do whatever I want MessagePlex to do.

But, Rx is too huge for small projects, as the last time I checked, MessagePlex builds into 16KB, 
 meanwhile even a single copy of System.Reactive.Interfaces.dll has a size of 24KB,
 and with the other parts of Rx the dependencies can easily grow over 800KB, sometimes the binary output can be over 1MB.

It also means there is no way a small client application wants to depend on Rx,
 especially in PRC where major part of users may suspect about what you are doing with this huge size.
 Even with Costura.Fody, the minimal possible output size is around 300KB if you use Rx-Linq.

So this is why I'm still working on this MessagePlex thing, while using & learning from Rx in my other projects.
