**Purpose**

When you need to know the difference between two .Net objects and save that difference for later. I've used this in the past when a server would distribute a default settings or object. If a user had made local changes, this library would be able to create a diff in order to save only what a user had changed, and then apply it to the new default settings object.

**How It Works**

This is a .Net application written in C#. It uses xmldiffpatch library from Microsoft. It also uses NUnit for the unit test suite.

The class is short and simple and should be obvious how to use. Internally it serializes objects to XML, then uses the xmldiffpatch tool from Microsoft to do the diff. xmldiffpatch has been reliable for me in the past so using it internally for this utility class made sense in order to cover all the use cases.

Of course, this means that only .Net objects that are xml serializable work with this utility.