﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace LlvmBindingsGenerator.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    internal partial class PerHeaderInteropTemplate : PerHeaderInteropTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// ------------------------------------------------------------------------------" +
                    "\r\n// <auto-generated>\r\n//     This code was generated by a tool.\r\n//     Runtime" +
                    " Version: ");
            
            #line 9 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ToolVersion));
            
            #line default
            #line hidden
            this.Write("\r\n//\r\n//     Changes to this file may cause incorrect behavior and will be lost i" +
                    "f\r\n//     the code is regenerated.\r\n// </auto-generated>\r\n// -------------------" +
                    "-----------------------------------------------------------\r\n#nullable disable\r\n" +
                    "\r\n");
            
            #line 17 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var import in Imports ) {
            
            #line default
            #line hidden
            this.Write("using ");
            
            #line 18 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(import));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 19 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\r\nnamespace Ubiquity.NET.Llvm.Interop\r\n{\r\n");
            
            #line 23 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var e in Enums){
            
            #line default
            #line hidden
            this.Write("    /// <include file=\"");
            
            #line 24 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(XDocIncludePath));
            
            #line default
            #line hidden
            this.Write("\" path=\'LibLlvmAPI/Enumeration[@name=\"");
            
            #line 24 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(e.Name));
            
            #line default
            #line hidden
            this.Write("\"]/*[not(self::Item)]\' />\r\n    [GeneratedCode(\"LlvmBindingsGenerator\",\"");
            
            #line 25 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ToolVersion));
            
            #line default
            #line hidden
            this.Write("\")]\r\n    public enum ");
            
            #line 26 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(e.Name));
            
            #line default
            #line hidden
            this.Write(" : ");
            
            #line 26 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(e.BaseType));
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n");
            
            #line 28 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var m in e.Members) {
            
            #line default
            #line hidden
            
            #line 29 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var commentLine in e.Comments){
            
            #line default
            #line hidden
            this.Write("        // ");
            
            #line 30 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(commentLine));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 31 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("        /// <include file=\"");
            
            #line 32 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(XDocIncludePath));
            
            #line default
            #line hidden
            this.Write("\" path=\'LibLlvmAPI/Enumeration[@name=\"");
            
            #line 32 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(e.Name));
            
            #line default
            #line hidden
            this.Write("\"]/Item[@name=\"");
            
            #line 32 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m.Name));
            
            #line default
            #line hidden
            this.Write("\"]/*\' />\r\n        ");
            
            #line 33 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m.Name));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 33 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(m.Value));
            
            #line default
            #line hidden
            this.Write(",\r\n");
            
            #line 34 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    }\r\n\r\n");
            
            #line 37 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
foreach(var d in Delegates) {
            
            #line default
            #line hidden
            
            #line 39 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var commentLine in d.Comments){
            
            #line default
            #line hidden
            this.Write("        // ");
            
            #line 40 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(commentLine));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 41 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    /// <include file=\"");
            
            #line 42 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(XDocIncludePath));
            
            #line default
            #line hidden
            this.Write("\" path=\'LibLlvmAPI/Delegate[@name=\"");
            
            #line 42 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(d.Name));
            
            #line default
            #line hidden
            this.Write("\"]/*\' />\r\n");
            
            #line 43 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach( var attrib in d.Attributes) {
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 44 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attrib.AsString()));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 45 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 46 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(d));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 47 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 48 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var s in ValueTypes){
     var parsedComment = new ParsedComment(s);
     foreach(var commentLine in parsedComment){
            
            #line default
            #line hidden
            this.Write("        // ");
            
            #line 51 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(commentLine));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 52 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    /// <include file=\"");
            
            #line 53 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(XDocIncludePath));
            
            #line default
            #line hidden
            this.Write("\" path=\'LibLlvmAPI/Struct[@name=\"");
            
            #line 53 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(s.Name));
            
            #line default
            #line hidden
            this.Write("\"]/*[not(self::Field)]\' />\r\n");
            
            #line 54 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
    foreach(var sa in s.Attributes) {
            
            #line default
            #line hidden
            this.Write("    ");
            
            #line 55 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(sa.AsString()));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 56 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    public struct ");
            
            #line 57 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(s.Name));
            
            #line default
            #line hidden
            this.Write("\r\n    {\r\n");
            
            #line 59 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var fld in s.Fields){
            
            #line default
            #line hidden
            this.Write("        /// <include file=\"");
            
            #line 60 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(XDocIncludePath));
            
            #line default
            #line hidden
            this.Write("\" path=\'LibLlvmAPI/struct[@name=\"");
            
            #line 60 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(s.Name));
            
            #line default
            #line hidden
            this.Write("\"]/Field[@name=\"");
            
            #line 60 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(fld.Name));
            
            #line default
            #line hidden
            this.Write("\"]/*\' />\r\n");
            
            #line 61 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
  foreach(var fa in fld.Attributes) {
            
            #line default
            #line hidden
            this.Write("        ");
            
            #line 62 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(fa.AsString()));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 63 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
  }
            
            #line default
            #line hidden
            this.Write("        public ");
            
            #line 64 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetManagedName(fld.Type)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 64 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(fld.Name));
            
            #line default
            #line hidden
            this.Write(";\r\n");
            
            #line 65 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    }\r\n\r\n");
            
            #line 68 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    public static partial class NativeMethods\r\n    {\r\n");
            
            #line 71 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var f in Functions) {
            
            #line default
            #line hidden
            this.Write("        /// <include file=\"");
            
            #line 72 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(XDocIncludePath));
            
            #line default
            #line hidden
            this.Write("\" path=\'LibLlvmAPI/Function[@name=\"");
            
            #line 72 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(f.Name));
            
            #line default
            #line hidden
            this.Write("\"]/*\' />\r\n");
            
            #line 73 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
foreach(var a in f.Attributes) {
            
            #line default
            #line hidden
            this.Write("        ");
            
            #line 74 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(a.AsString()));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 75 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("        ");
            
            #line 76 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(f));
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 77 "D:\GitHub\Ubiquity.NET\Llvm.Net\src\Interop\LlvmBindingsGenerator\Templates\T4\PerHeaderInteropTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    }\r\n}\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    internal class PerHeaderInteropTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
