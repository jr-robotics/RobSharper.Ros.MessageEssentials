# ROS Package XML Data Contract

## How to Use

```CSharp
var serializer = new XmlSerializer(typeof(Joanneum.Robotics.Ros.PackageXml.V2.package));
var package = (Joanneum.Robotics.Ros.PackageXml.V2.package)serializer.Deserialize(new XmlTextReader(filename));
```


## How to update data contracts

Data files are generated from ROS Package XSD Files using Microsoft xsd.exe (https://docs.microsoft.com/en-us/dotnet/standard/serialization/xml-schema-definition-tool-xsd-exe).
ROS XSD Files are located in the VX (e.g. V2) folder. 

Use ```build.ps1``` powershell script to generate  a new version of the package.
The script takes the path to xsd.exe as only argument

**Example - How to generate package.xml V2 code in powerhsell:**

```powershell
.\build.ps1 "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools\xsd.exe"
``` 

### Resources

- http://wiki.ros.org/catkin/package.xml
- http://download.ros.org/schema/
- http://download.ros.org/schema/package_format1.xsd
- http://download.ros.org/schema/package_format2.xsd
- http://download.ros.org/schema/package_format3.xsd