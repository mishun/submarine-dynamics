module SubDyn.Math.Xml

open System
open System.IO
open System.Xml
open System.Xml.XPath

let internal writeAttr (xml : XmlWriter) (name : string, value : float) =
    xml.WriteStartAttribute name
    xml.WriteValue value
    xml.WriteEndAttribute ()

let serializeToXml (stream : Stream) name writer =
    let settings = XmlWriterSettings ( Indent = true, IndentChars = "\t" )
    using (XmlTextWriter.Create (stream, settings)) <| fun xml ->
        xml.WriteStartDocument ()
        xml.WriteStartElement name
        writer xml
        xml.WriteEndElement ()
        xml.WriteEndDocument ()

let serializeFloat (xml : XmlWriter) name (v : float) =
    xml.WriteStartElement name
    xml.WriteValue v
    xml.WriteEndElement ()

let serializeVector2 (xml : XmlWriter) name (v : Vector2) =
    xml.WriteStartElement name
    Array.iter (writeAttr xml) [| ("X", v.X); ("Y", v.Y) |]
    xml.WriteEndElement ()

let serializeVector3 (xml : XmlWriter) name (v : Vector3) =
    xml.WriteStartElement name
    Array.iter (writeAttr xml) [| ("X", v.X); ("Y", v.Y); ("Z", v.Z) |]
    xml.WriteEndElement ()

let serializeOrientation (xml : XmlWriter) name (o : Orientation) =
    xml.WriteStartElement name
    Array.iter (writeAttr xml) [| ("Roll", o.Roll); ("Pitch", o.Pitch); ("Yaw", o.Yaw) |]
    xml.WriteEndElement ()

let serializeMatrix2 (xml : XmlWriter) name (m : Matrix2) =
    xml.WriteStartElement name
    Array.iter (writeAttr xml) [| ("XX", m.XX); ("XY", m.XY); ("YX", m.YX); ("YY", m.YY); |]
    xml.WriteEndElement ()

let serializeMatrix3 (xml : XmlWriter) name (m : Matrix3) =
    xml.WriteStartElement name
    Array.iter (writeAttr xml) [| ("XX", m.XX); ("XY", m.XY); ("XZ", m.XZ); ("YX", m.YX); ("YY", m.YY); ("YZ", m.YZ); ("ZX", m.ZX); ("ZY", m.ZY); ("ZZ", m.ZZ); |]
    xml.WriteEndElement ()


let deserialize (xml : XPathNavigator) (xpath : string) parser =
    let it = xml.Select xpath
    if it.MoveNext ()
        then
            let result = parser it.Current
            if it.MoveNext ()
                then raise <| FormatException ("Multiple nodes satifying query '" + xpath + "'")
                else Some result
        else None

let forceDeserialize xml xpath parser =
    match deserialize xml xpath parser with
        | Some value -> value
        | None       -> raise <| FormatException ("No nodes satisfying query '" + xpath + "'")

let deserializeAction xml xpath parser action =
    match deserialize xml xpath parser with
        | Some value -> action value
        | None       -> ()

let parseFloat (xml : XPathNavigator) =
    xml.ValueAsDouble

let parseVector2 xml =
    let x = forceDeserialize xml "./@X" parseFloat
    let y = forceDeserialize xml "./@Y" parseFloat
    Vector2 (x, y)

let parseVector3 xml =
    let x = forceDeserialize xml "./@X" parseFloat
    let y = forceDeserialize xml "./@Y" parseFloat
    let z = forceDeserialize xml "./@Z" parseFloat
    Vector3 (x, y, z)

let parseOrientation xml =
    let roll = forceDeserialize xml "./@Roll" parseFloat
    let pitch = forceDeserialize xml "./@Pitch" parseFloat
    let yaw = forceDeserialize xml "./@Yaw" parseFloat
    Orientation.FromRollPitchYaw (roll, pitch, yaw)

let parseMatrix2 xml =
    let xx = forceDeserialize xml "./@XX" parseFloat
    let xy = forceDeserialize xml "./@XY" parseFloat
    let yx = forceDeserialize xml "./@YX" parseFloat
    let yy = forceDeserialize xml "./@YY" parseFloat
    Matrix2 (xx, xy, yx, yy)

let parseMatrix3 xml =
    let xx = forceDeserialize xml "./@XX" parseFloat
    let xy = forceDeserialize xml "./@XY" parseFloat
    let xz = forceDeserialize xml "./@XZ" parseFloat
    let yx = forceDeserialize xml "./@YX" parseFloat
    let yy = forceDeserialize xml "./@YY" parseFloat
    let yz = forceDeserialize xml "./@YZ" parseFloat
    let zx = forceDeserialize xml "./@ZX" parseFloat
    let zy = forceDeserialize xml "./@ZY" parseFloat
    let zz = forceDeserialize xml "./@ZZ" parseFloat
    Matrix3 (xx, xy, xz, yx, yy, yz, zx, zy, zz)
