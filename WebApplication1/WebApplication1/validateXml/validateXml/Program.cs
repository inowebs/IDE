using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;        // para XmlTextReader y XmlValidatingReader
using System.Xml.Schema; // para XmlSchemaCollection (que se utiliza más adelante)

namespace validateXml
{
    class Program
    {
        private static bool isValid = true; 

        static void Main(string[] args)
        {
            XmlTextReader r = new XmlTextReader("C:\\SAT\\123456\\A-2014N.xml");
            XmlValidatingReader v = new XmlValidatingReader(r);
            v.ValidationType = ValidationType.Schema;
            v.ValidationEventHandler += new ValidationEventHandler(MyValidationEventHandler);
            while (v.Read())
            {
               // Puede agregar código aquí para procesar el contenido.
            }
            v.Close();

            // Comprobar si el documento es válido o no.
            if (isValid) 
               Console.WriteLine("El documento es válido");
            else
               Console.WriteLine("El documento no es válido");
            Console.ReadLine();

        }

        public static void MyValidationEventHandler(object sender,
                                                    ValidationEventArgs args)
        {
            isValid = false;
            Console.WriteLine("Evento de validación\n" + args.Message);
        }
    }
}
