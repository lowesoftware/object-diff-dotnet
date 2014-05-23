using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.XmlDiffPatch;
using System.IO;

namespace ObjectDiff
{
    public class ObjectDifference
    {

        public string Difference(object original, object changed)
        {
            XmlReader originalreader = XmlReader.Create(new StringReader(Serialize(original)));
            XmlReader changedreader = XmlReader.Create(new StringReader(Serialize(changed)));

            StringBuilder patch = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(patch);

            XmlDiff xmldiff = new XmlDiff(XmlDiffOptions.IgnoreChildOrder |
                                             XmlDiffOptions.IgnoreNamespaces |
                                             XmlDiffOptions.IgnorePrefixes);

            bool bIdentical = xmldiff.Compare(originalreader, changedreader, writer);
            writer.Close();

            return patch.ToString();
        }


        // public void PatchUp(string originalFile, String diffGramFile, String OutputFile)
        public object Patch(object source, string patch)
        {
            XmlReader readersource = XmlReader.Create(new StringReader(Serialize(source)));
            

            XmlDocument docsource = new XmlDocument(new NameTable());
            docsource.Load(readersource);
            XmlTextReader readerpatch = new XmlTextReader(new StringReader(patch));

            XmlPatch xmlpatch = new XmlPatch();

            xmlpatch.Patch(docsource, readerpatch);

            StringBuilder newxml = new StringBuilder();
            XmlWriter writernewxml = XmlWriter.Create(newxml);

            docsource.Save(writernewxml);

            return Deserialize(newxml.ToString(), source.GetType());
        }



        private string Serialize(object o)
        {
            StringBuilder buffer = new StringBuilder();

            // Serialization
            XmlSerializer s = new XmlSerializer(o.GetType());
            using (StringWriter w = new StringWriter(buffer))
            {
                s.Serialize(w, o);
            }

            return buffer.ToString();

        }

        private object Deserialize(string xml, Type type)
        {
            XmlSerializer s = new XmlSerializer(type);
            using (StringReader r = new StringReader(xml))
            {
                object o = s.Deserialize(r);
                return o;
            }
        }

    }
}
