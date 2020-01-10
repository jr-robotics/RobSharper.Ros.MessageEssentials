# ROS Package XML Data Contract

## How to Use

```CSharp
var serializer = new XmlSerializer(typeof(Joanneum.Robotics.Ros.PackageXml.V2.package));
var package = (Joanneum.Robotics.Ros.PackageXml.V2.package)serializer.Deserialize(new XmlTextReader(filename));
```


## How to update data contracts

Data files are generated from ROS Package XSD Files using Microsoft xsd.exe (https://docs.microsoft.com/en-us/dotnet/standard/serialization/xml-schema-definition-tool-xsd-exe).
ROS XSD Files are located in the VX (e.g. V2) folders. 

###Example: How to generate package.xml V2 code in **powerhsell**:


```powershell
# SET Path to xsd.exe
$xsdPath = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\xsd.exe"

# Go to V2 folder
cd V2

# Start Code generation
& $xsdPath package_format2.xsd /parameters:xsdOptions.xml

# Rebuild solution
cd ..
dotnet build
``` 

### Resources

- http://wiki.ros.org/catkin/package.xml
- http://download.ros.org/schema/package_format2.xsd