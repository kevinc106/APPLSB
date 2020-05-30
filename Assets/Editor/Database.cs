using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;

namespace CreationStates
{
    public class Database
    {
        public List<object> expressions { get; set; } = new List<object>();
        public ArrayList dictionary { get; set; }
        
        public Database(String categoryPath,String configPath)
        { 
            initializeCategoryDictionary(categoryPath);
            ArrayList data;
            using (StreamReader reader = new StreamReader(configPath))
            {
                string json = reader.ReadToEnd();
                data = JsonConvert.DeserializeObject<ArrayList>(json);
                foreach (var item in data)
                {
                    var moduleExpressions = loadModule((JObject)item);
                    expressions.AddRange(moduleExpressions);
                } 
            } 
        }

        private List<object> loadModule(JObject module)
        {
            List<object> response =new List<object>();
            foreach (var category in module["categories"])
            {
                //Debug.Log(category);
                try
                {
                    String categoryCode = getCategoryCode(category["name"]);
                    if (categoryCode==null)
                    {
                        throw new Exception("Not valid category");
                    }
                    else
                    {
                        var resourcesFolder = "Assets/Resources/";
                        var yamlFilePath = String.Format("DataBase/data/{0}/{1}.yaml", module["name"].ToString(), category["name"].ToString());
                        var path = Path.Combine(resourcesFolder, @yamlFilePath);
                        List<ExpressionData> expressionsData = new List<ExpressionData>();
                        using (var reader = new StreamReader(path,Encoding.UTF8))
                        {
                            // Load the stream
                            expressionsData = GetExpressionsFromReader(reader); 
                            foreach (ExpressionData expression in expressionsData)
                            {
                               expression.updateExpression(categoryCode, category["set"].ToString());

                            }
                            response.AddRange(expressionsData);
                            Debug.Log("OK - Load " + $"./data/{module["name"]}/{category["name"]}.yaml");
                        }
                    }
                } catch(Exception error)
                {
                    Debug.Log("ERROR - Load " + $"./data/{module["name"]}/{category["name"]}.yaml"+"\n"+ error.Message);

                }
            } 
            return response;
        }

        private static List<ExpressionData> GetExpressionsFromReader(StreamReader reader)
        {
            YamlStream yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var node = yamlStream.Documents[0].RootNode;
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            var expressionsData = deserializer.Deserialize<List<ExpressionData>>(
                new EventStreamParserAdapter(
                    YamlNodeToEventStreamConverter.ConvertToEventStream(node)
                )
            );
            return expressionsData;
        }

        private string getCategoryCode(object categoryName)
        {
            foreach(JObject category in dictionary)
            {
                if (category["name"].ToString() == categoryName.ToString())
                {
                    return category["code"].ToString();
                }
            }
            return null;
        }

        private void initializeCategoryDictionary(string categoryPath)
        { 
            try
            {
                using (StreamReader r = new StreamReader(categoryPath))
                {
                    string json = r.ReadToEnd();
                    dictionary = JsonConvert.DeserializeObject<ArrayList>(json); 
                }
            }
            catch (Exception er)
            {
                Debug.Log(er.Message);
            }
        }


    }
}
