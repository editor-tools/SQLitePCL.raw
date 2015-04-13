/*
   Copyright 2014-2015 Zumero, LLC

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public static class projects
{
	// Each item in these Lists corresponds to a project file that will be
	// generated.
	//
	public static List<config_sqlite3> items_sqlite3 = new List<config_sqlite3>();
	public static List<config_cppinterop> items_cppinterop = new List<config_cppinterop>();
	public static List<config_pcl> items_pcl = new List<config_pcl>();
	public static List<config_higher> items_higher = new List<config_higher>();
	public static List<config_tests> items_tests = new List<config_tests>();

	// This function is called by Main to initialize the project lists.
	//
	public static void init()
	{
		init_sqlite3(false);
		init_sqlite3(true);
		init_cppinterop(false);
		init_cppinterop(true);
		init_pcl_bait();
		init_pcl_pinvoke();
		init_pcl_cppinterop(false);
		init_pcl_cppinterop(true);
		init_higher();
		init_tests();
	}

	private static void init_sqlite3(bool dyn)
	{
		items_sqlite3.Add(new config_sqlite3 { env="winxp", cpu="x86", dll=dyn });
		items_sqlite3.Add(new config_sqlite3 { env="winxp", cpu="x64", dll=dyn });
	}

	private static void init_cppinterop(bool dyn)
	{
		items_cppinterop.Add(new config_cppinterop { env="net45", cpu="x64", dll=dyn });
		items_cppinterop.Add(new config_cppinterop { env="net45", cpu="x86", dll=dyn });

	}

	private static void init_pcl_bait()
	{
		items_pcl.Add(new config_pcl { env="profile78", cpu="anycpu" });
		items_pcl.Add(new config_pcl { env="profile259", cpu="anycpu" });
		items_pcl.Add(new config_pcl { env="profile158", cpu="anycpu" });
		items_pcl.Add(new config_pcl { env="profile111", cpu="anycpu" });
	}

	private static void init_tests()
	{
		// TODO it is not confirmed that any other environments will work here.
		// for now that's fine.

		items_tests.Add(new config_tests { env="net45", pcl="profile78" });
	}

	private static void init_higher()
	{
		items_higher.Add(new config_higher { name="ugly", assemblyname="SQLitePCL.ugly", env="profile78", csfiles=new List<string>() {"src\\cs\\ugly.cs"} });
		items_higher.Add(new config_higher { name="ugly", assemblyname="SQLitePCL.ugly", env="profile111", csfiles=new List<string>() {"src\\cs\\ugly.cs"} });
		items_higher.Add(new config_higher { name="ugly", assemblyname="SQLitePCL.ugly", env="profile259", csfiles=new List<string>() {"src\\cs\\ugly.cs"} });
	}

	private static void init_pcl_cppinterop(bool dyn)
	{
		items_pcl.Add(new config_pcl { env="net45", api="cppinterop", what="sqlite3", cpu="x86", dll=dyn});
		items_pcl.Add(new config_pcl { env="net45", api="cppinterop", what="sqlite3", cpu="x64", dll=dyn});

	}

	private static void init_pcl_pinvoke()
	{

		items_pcl.Add(new config_pcl { env="net45", api="pinvoke", what="sqlite3", cpu="anycpu"});
		items_pcl.Add(new config_pcl { env="net45", api="pinvoke", what="sqlite3", cpu="x86"});
		items_pcl.Add(new config_pcl { env="net45", api="pinvoke", what="sqlite3", cpu="x64"});
		items_pcl.Add(new config_pcl { env="net45", api="pinvoke", what="packaged_sqlite3", cpu="anycpu"});


#if not
		items_pcl.Add(new config_pcl { env="wp81_sl", api="pinvoke", what="sqlite3", cpu="anycpu"});
		items_pcl.Add(new config_pcl { env="wp81_sl", api="pinvoke", what="sqlite3", cpu="arm"});
		items_pcl.Add(new config_pcl { env="wp81_sl", api="pinvoke", what="sqlite3", cpu="x86"});

		items_pcl.Add(new config_pcl { env="wp80", api="pinvoke", what="sqlite3", cpu="anycpu"});
		items_pcl.Add(new config_pcl { env="wp80", api="pinvoke", what="sqlite3", cpu="arm"});
		items_pcl.Add(new config_pcl { env="wp80", api="pinvoke", what="sqlite3", cpu="x86"});
#endif
	}

	public static string get_portable_nuget_target_string(string env)
	{
		switch (env)
		{
			case "profile78":
				return "portable-net45+netcore45+wp8+MonoAndroid10+MonoTouch10+Xamarin.iOS10";
			case "profile259":
				return "portable-net45+netcore45+wpa81+wp8+MonoAndroid10+MonoTouch10+Xamarin.iOS10";
			case "profile111":
				return "portable-net45+netcore45+wpa81+MonoAndroid10+MonoTouch10+Xamarin.iOS10";
			case "profile158":
				return "portable-net45+sl5+netcore45+wp8+MonoAndroid10+MonoTouch10+Xamarin.iOS10";
			default:
				throw new Exception(env);
		}
	}

	public static config_sqlite3 find_sqlite3(string env, string cpu, bool dll)
	{
		foreach (config_sqlite3 cfg in projects.items_sqlite3)
		{
			if (
					(cfg.env == env)
					&& (cfg.cpu == cpu)
					&& (cfg.dll == dll)
			   )
			{
				return cfg;
			}
		}
		//throw new Exception();
		return null;
	}

	public static config_pcl find_bait(string env)
	{
		foreach (config_pcl cfg in projects.items_pcl)
		{
			if (cfg.env == env)
			{
				return cfg;
			}
		}
		throw new Exception(env);
	}

	public static config_higher find_ugly(string env)
	{
		foreach (config_higher cfg in projects.items_higher)
		{
			if (
					(cfg.name == "ugly")
					&& (cfg.env == env)
			   )
			{
				return cfg;
			}
		}
		throw new Exception(env);
	}

	public static List<config_pcl> find_pcls(string env, string api, string what, string cpu, string linkage)
	{
		List<config_pcl> a = new List<config_pcl>();
		foreach (config_pcl cfg in projects.items_pcl)
		{
			if (
					(env != null)
					&& (cfg.env != env)
			   )
			{
				continue;
			}

			if (
					(api != null)
					&& (cfg.api != api)
			   )
			{
				continue;
			}

			if (
					(what != null)
					&& (cfg.what != what)
			   )
			{
				continue;
			}

			if (
					(cpu != null)
					&& (cfg.cpu != cpu)
			   )
			{
				continue;
			}

			if (linkage != null)
			{
				if (
						(linkage == "static")
						&& (cfg.dll)
				   )
				{
					continue;
				}
				if (
						(linkage == "dynamic")
						&& (!cfg.dll)
				   )
				{
					continue;
				}
			}

			a.Add(cfg);
		}
		return a;
	}

}

public interface config_info
{
	string get_project_filename();
	string get_name();
	string get_dest_subpath();
}

public class config_sqlite3 : config_info
{
	public string env;
	public string cpu;
	public string guid;
	public bool dll = false;

	private void add_product(List<string> a, string s)
	{
		a.Add(Path.Combine(get_dest_subpath(), s));
	}

	public void get_products(List<string> a)
	{
		if (dll)
		{
			// TODO this is Windows-specific
			add_product(a, "sqlite3.dll");
		}
	}

	private string area()
	{
		return "sqlite3_" + (dll ? "dynamic" : "static");
	}

	public string get_nuget_target_path()
	{
		return string.Format("build\\native\\{0}\\", get_dest_subpath());
	}

	public string get_dest_subpath()
	{
		return string.Format("{0}\\{1}\\{2}", area(), env, cpu);
	}

	public string get_name()
	{
		return string.Format("{0}.{1}.{2}", area(), env, cpu);
	}

	public string get_project_filename()
	{
		return string.Format("{0}.vcxproj", get_name());
	}

	public string fixed_cpu()
	{
		if (cpu == "x86")
		{
			return "Win32";
		}
		else
		{
			return cpu;
		}
	}
}

public class config_cppinterop : config_info
{
	public string env;
	public string cpu;
	public string guid;
	public bool dll = false;

	public config_sqlite3 get_sqlite3_item()
	{
		config_sqlite3 other = projects.find_sqlite3(env=="net45"?"winxp":env, cpu, dll);
		if (other == null)
		{
			throw new Exception(get_name());
		}
		return other;
	}

	private void add_product(List<string> a, string s)
	{
		a.Add(Path.Combine(get_dest_subpath(), s));
	}

	public void get_products(List<string> a)
	{
		add_product(a, "SQLitePCL.cppinterop.dll");
	}

	private string area()
	{
		return "cppinterop_sqlite3_" + (dll ? "dynamic" : "static");
	}

	public string get_name()
	{
		return string.Format("{0}.{1}.{2}", area(), env, cpu);
	}

	public string get_dest_subpath()
	{
		return string.Format("{0}\\{1}\\{2}", area(), env, cpu);
	}

	public string get_project_filename()
	{
		return string.Format("{0}.vcxproj", get_name());
	}

	public string fixed_cpu()
	{
		if (cpu == "x86")
		{
			return "Win32";
		}
		else
		{
			return cpu;
		}
	}

	private string sqlite3_env()
	{
		if (env == "net45")
		{
			return "winxp";
		}
		else
		{
			return env;
		}
	}
}

public class config_higher : config_info
{
	public string env;
	public string name;
	public string assemblyname;
	public string guid;
	public List<string> csfiles;
	public List<string> defines;

	public string get_dest_subpath()
	{
		return string.Format("{0}\\{1}", name, env);
	}

	public string get_name()
	{
		return string.Format("{0}_{1}", name, env);
	}

	public string get_project_filename()
	{
		return string.Format("{0}.csproj", get_name());
	}

	private void add_product(List<string> a, string s)
	{
		a.Add(Path.Combine(get_dest_subpath(), s));
	}

	public string get_nuget_target_path()
	{
		return string.Format("lib\\{0}\\", projects.get_portable_nuget_target_string(env));
	}

	public void get_products(List<string> a)
	{
		add_product(a, string.Format("{0}.dll", assemblyname));
	}
}

public class config_tests : config_info
{
	public string env;
	public string pcl;
	public string guid;

	public string get_project_filename()
	{
		return string.Format("{0}.csproj", get_name());
	}

	public string get_dest_subpath()
	{
		return string.Format("{0}\\{1}", "tests", env);
	}

	public string get_name()
	{
		return string.Format("{0}_{1}", "tests", env);
	}

}

public class config_pcl : config_info
{
	public string env;
	public string api;
	public string what;
	public string cpu;
	public string guid;
	public bool dll; // TODO should be string linkage, so it can be null for cases where it is not used

	public config_sqlite3 get_sqlite3_item()
	{
		if (is_cppinterop() && dll)
		{
			config_cppinterop other = get_cppinterop_item();
			return other.get_sqlite3_item();
		}

		if (is_pinvoke())
		{
			config_sqlite3 other = projects.find_sqlite3(env=="net45"?"winxp":env, cpu, true);
			return other;
		}

		return null;
	}

	public config_cppinterop get_cppinterop_item()
	{
		foreach (config_cppinterop cfg in projects.items_cppinterop)
		{
			if (
					(cfg.env == env)
					&& (cfg.cpu == cpu)
					&& (cfg.dll == dll)
			   )
			{
				return cfg;
			}
		}
		throw new Exception(get_name());
	}

	public static string get_nuget_framework_name(string env)
	{
		// TODO maybe I should just use these names?
		switch (env)
		{
			case "net45":
				return "net45";
			default:
				throw new Exception(env);
		}
	}
					
	private string nat()
	{
		if (is_pinvoke())
		{
			return string.Format("{0}_{1}", api, what);
		}
		else if (is_cppinterop())
		{
			return string.Format("{0}_{1}_{2}", api, what, (dll ? "dynamic" : "static"));
		}
		else
		{
			throw new Exception(get_name());
		}
	}

	public string get_nuget_target_subpath()
	{
		return string.Format("{0}\\{1}", nat(), cpu);
	}

	public string get_nuget_target_path(string where)
	{
		if ("build" == where)
		{
			if (is_portable())
			{
				throw new Exception(get_name());
			}
			else
			{
				return string.Format("build\\{0}\\{1}\\{2}\\", get_nuget_framework_name(env), nat(), cpu);
			}
		}
		else if ("lib" == where)
		{
			if (is_portable())
			{
				return string.Format("lib\\{0}\\", projects.get_portable_nuget_target_string(env));
			}
			else
			{
				return string.Format("lib\\{0}\\", get_nuget_framework_name(env));
			}
		}
		else
		{
			throw new Exception(get_name());
		}
	}

	private void add_product(List<string> a, string s)
	{
		a.Add(Path.Combine(get_dest_subpath(), s));
	}

	public void get_products(List<string> a)
	{
		add_product(a, "SQLitePCL.raw.dll");

		if (is_cppinterop())
		{
			config_cppinterop other = get_cppinterop_item();
			other.get_products(a);
		}
	}

	public bool is_portable()
	{
		return env.StartsWith("profile");
	}

	public bool is_cppinterop()
	{
		if (is_portable())
		{
			return false;
		}

		return api == "cppinterop";
	}

	public bool is_pinvoke()
	{
		if (is_portable())
		{
			return false;
		}

		return api == "pinvoke";
	}

	private const string AREA = "pcl";

	public string get_dest_subpath()
	{
		if (is_portable())
		{
			return string.Format("{0}\\{1}", AREA, env);
		}
		else
		{
			return string.Format("{0}\\{1}\\{2}\\{3}", AREA, env, nat(), cpu);
		}
	}

	public string get_name()
	{
		if (is_portable())
		{
			return string.Format("portable.{0}", env);
		}
		else
		{
			return string.Format("platform.{0}.{1}.{2}", env, nat(), cpu);
		}
	}

	public string get_project_filename()
	{
		return string.Format("{0}.csproj", get_name());
	}
	
	public string fixed_cpu()
	{
		if (cpu == "anycpu")
		{
			return "Any CPU";
		}
		else
		{
			return cpu;
		}
	}
}

public static class gen
{
	private const string GUID_CSHARP = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
	private const string GUID_CPP = "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";
	private const string GUID_FOLDER = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
	private const string GUID_PCL = "{786C830F-07A1-408B-BD7F-6EE04809D6DB}";
	private const string GUID_TEST = "{3AC096D0-A1C2-E12C-1390-A8335801FDAB}";

	private static void write_reference(XmlWriter f, string s)
	{
		f.WriteStartElement("Reference");
		f.WriteAttributeString("Include", s);
		f.WriteEndElement(); // Reference
	}

	private static void write_cs_compile(XmlWriter f, string root, string s)
	{
		string path = Path.Combine(root, s);
		f.WriteStartElement("Compile");
		f.WriteAttributeString("Include", path);
		f.WriteEndElement(); // Compile
	}

	private static void write_cpp_includepath(XmlWriter f, string root, string s)
	{
		f.WriteElementString("IncludePath", string.Format("{0};$(IncludePath)", Path.Combine(root, s)));
	}

	private static void write_cpp_define(XmlWriter f, string s)
	{
		f.WriteElementString("PreprocessorDefinitions", string.Format("{0};%(PreprocessorDefinitions)", s));
	}

	private static void write_project_type_guids(XmlWriter f, string s1)
	{
		f.WriteElementString("ProjectTypeGuids", string.Format("{0}", s1));
	}

	private static void write_project_type_guids(XmlWriter f, string s1, string s2)
	{
		f.WriteElementString("ProjectTypeGuids", string.Format("{0};{1}", s1, s2));
	}

	private static void write_section(string dest_subpath, XmlWriter f, bool debug, List<string> defines)
	{
		string name = debug ? "debug" : "release";
		f.WriteStartElement("PropertyGroup");
		f.WriteAttributeString("Condition", string.Format(" '$(Configuration)' == '{0}' ", name));

		f.WriteElementString("OutputPath", string.Format("{0}\\bin\\{1}\\", name, dest_subpath));
		f.WriteElementString("IntermediateOutputPath", string.Format("{0}\\obj\\{1}\\", name, dest_subpath));

		string defs;
		if (debug)
		{
			defs = "DEBUG;";
		}
		else
		{
			defs = "";
		}
		if (defines != null)
		{
			foreach (string d in defines)
			{
				defs += d;
				defs += ";";
			}
		}

		if (defs.Length > 0)
		{
			f.WriteElementString("DefineConstants", defs);
		}

		f.WriteElementString("DebugSymbols", debug ? "true" : "false");
		f.WriteElementString("Optimize", debug ? "false" : "true");
		f.WriteElementString("DebugType", debug ? "full" : "pdbonly");

		f.WriteEndElement(); // PropertyGroup
	}

	private static void write_section(config_pcl cfg, XmlWriter f, bool debug, List<string> defines)
	{
		string name = debug ? "debug" : "release";
		f.WriteStartElement("PropertyGroup");
		f.WriteAttributeString("Condition", string.Format(" '$(Configuration)|$(Platform)' == '{0}|{1}' ", name, cfg.cpu));

		f.WriteElementString("OutputPath", string.Format("{0}\\bin\\{1}\\", name, cfg.get_dest_subpath()));
		f.WriteElementString("IntermediateOutputPath", string.Format("{0}\\obj\\{1}\\", name, cfg.get_dest_subpath()));

		string defs;
		if (debug)
		{
			defs = "DEBUG;";
		}
		else
		{
			defs = "";
		}
		foreach (string d in defines)
		{
			defs += d;
			defs += ";";
		}
		f.WriteElementString("DefineConstants", defs);

		f.WriteElementString("DebugSymbols", debug ? "true" : "false");
		f.WriteElementString("Optimize", debug ? "false" : "true");
		f.WriteElementString("DebugType", debug ? "full" : "pdbonly");

		f.WriteEndElement(); // PropertyGroup
	}

	private static void gen_sqlite3(config_sqlite3 cfg, string root, string top)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, cfg.get_project_filename()), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			f.WriteAttributeString("ToolsVersion", ToolsVersion);
			f.WriteAttributeString("DefaultTargets", "Build");

			f.WriteStartElement("ItemGroup");
			f.WriteAttributeString("Label", "ProjectConfigurations");

			f.WriteStartElement("ProjectConfiguration");
			f.WriteAttributeString("Include", string.Format("Debug|{0}", cfg.fixed_cpu()));
			f.WriteElementString("Configuration", "Debug");
			f.WriteElementString("Platform", cfg.fixed_cpu());
			f.WriteEndElement(); // ProjectConfiguration

			f.WriteStartElement("ProjectConfiguration");
			f.WriteAttributeString("Include", string.Format("Release|{0}", cfg.fixed_cpu()));
			f.WriteElementString("Configuration", "Release");
			f.WriteElementString("Platform", cfg.fixed_cpu());
			f.WriteEndElement(); // ProjectConfiguration

			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("PropertyGroup");
			f.WriteElementString("ProjectGuid", cfg.guid);
			f.WriteElementString("Keyword", "Win32Proj");
			f.WriteElementString("DefaultLanguage", "en-us");

			switch (cfg.env)
			{
				case "winxp":
					break;
			}

			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(VCTargetsPath)\\Microsoft.Cpp.Default.props");
			f.WriteEndElement(); // Import

			f.WriteStartElement("PropertyGroup");
			if (cfg.dll)
			{
				f.WriteElementString("ConfigurationType", "DynamicLibrary");
			}
			else
			{
				f.WriteElementString("ConfigurationType", "StaticLibrary");
			}
			f.WriteElementString("TargetName", "sqlite3");

			switch (cfg.env)
			{
				case "winxp":
					f.WriteElementString("PlatformToolset", PlatformToolset + "_xp");
					break;
			}

			f.WriteEndElement(); // PropertyGroup

			switch (cfg.env)
			{
				case "winxp":
					break;
			}

			switch (cfg.env)
			{
				case "winxp":
					break;
			}

			f.WriteStartElement("PropertyGroup");
			f.WriteAttributeString("Condition", string.Format(" '$(Configuration)' == 'Debug' "));
			f.WriteElementString("UseDebugLibraries", "true");
			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("PropertyGroup");
			f.WriteAttributeString("Condition", string.Format(" '$(Configuration)' == 'Release' "));
			f.WriteElementString("UseDebugLibraries", "false");
			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(VCTargetsPath)\\Microsoft.Cpp.props");
			f.WriteEndElement(); // Import

			f.WriteStartElement("PropertyGroup");
			f.WriteElementString("OutDir", string.Format("$(Configuration)\\bin\\{0}\\", cfg.get_dest_subpath()));
			f.WriteElementString("IntDir", string.Format("$(Configuration)\\obj\\{0}\\", cfg.get_dest_subpath()));
			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("ItemDefinitionGroup");
			f.WriteStartElement("ClCompile");
			if (cfg.dll)
			{
				write_cpp_define(f, "_USRDLL");
				write_cpp_define(f, "SQLITE_API=__declspec(dllexport)");
			}
			else
			{
				write_cpp_define(f, "_LIB");
			}
			//write_cpp_define(f, "SQLITE_OMIT_LOAD_EXTENSION");
			//write_cpp_define(f, "SQLITE_THREADSAFE=whatever");
			//write_cpp_define(f, "SQLITE_TEMP_STORE=whatever");
			write_cpp_define(f, "SQLITE_DEFAULT_FOREIGN_KEYS=1");
			//write_cpp_define(f, "SQLITE_ENABLE_RTREE");
			write_cpp_define(f, "SQLITE_ENABLE_FTS4");
			write_cpp_define(f, "SQLITE_ENABLE_FTS3_PARENTHESIS");
			write_cpp_define(f, "SQLITE_ENABLE_COLUMN_METADATA");
			f.WriteElementString("PrecompiledHeader", "NotUsing");
			f.WriteElementString("CompileAsWinRT", "false");
			f.WriteElementString("SDLCheck", "false");
			f.WriteEndElement(); // ClCompile
			f.WriteStartElement("Link");
			f.WriteElementString("SubSystem", "Console");
			f.WriteElementString("IgnoreAllDefaultLibraries", "false");
			f.WriteElementString("GenerateWindowsMetadata", "false");
			f.WriteEndElement(); // Link
			f.WriteEndElement(); // ItemDefinitionGroup

			f.WriteStartElement("ItemDefinitionGroup");
			f.WriteAttributeString("Condition", string.Format("'$(Configuration)'=='{0}' ", "Debug"));
			f.WriteStartElement("ClCompile");
			f.WriteElementString("Optimization", "Disabled");
			write_cpp_define(f, "_DEBUG");
			f.WriteEndElement(); // ClCompile
			f.WriteStartElement("Link");
			f.WriteElementString("GenerateDebugInformation", "true");
			f.WriteEndElement(); // Link
			f.WriteEndElement(); // ItemDefinitionGroup

			f.WriteStartElement("ItemDefinitionGroup");
			f.WriteStartElement("ClCompile");
			f.WriteAttributeString("Condition", string.Format("'$(Configuration)'=='{0}' ", "Release"));
			f.WriteElementString("Optimization", "MaxSpeed");
			f.WriteElementString("FunctionLevelLinking", "true");
			f.WriteElementString("IntrinsicFunctions", "true");
			write_cpp_define(f, "NDEBUG");
			f.WriteEndElement(); // ClCompile
			f.WriteStartElement("Link");
			f.WriteElementString("GenerateDebugInformation", "false");
			f.WriteElementString("EnableCOMDATFolding", "true");
			f.WriteElementString("OptimizeReferences", "true");
			f.WriteEndElement(); // Link
			f.WriteEndElement(); // ItemDefinitionGroup

			f.WriteStartElement("ItemGroup");
			{
				string path = Path.Combine(root, "sqlite3\\sqlite3.c");
				f.WriteStartElement("ClCompile");
				f.WriteAttributeString("Include", path);
				f.WriteEndElement(); // ClCompile
			}
			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(VCTargetsPath)\\Microsoft.Cpp.targets");
			f.WriteEndElement(); // Import

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	private static void gen_cppinterop(config_cppinterop cfg, string root, string top)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, cfg.get_project_filename()), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			switch (cfg.env)
			{
				default:
					f.WriteAttributeString("ToolsVersion", ToolsVersion);
					break;
			}
			f.WriteAttributeString("DefaultTargets", "Build");

			f.WriteStartElement("ItemGroup");
			f.WriteAttributeString("Label", "ProjectConfigurations");

			f.WriteStartElement("ProjectConfiguration");
			f.WriteAttributeString("Include", string.Format("Debug|{0}", cfg.fixed_cpu()));
			f.WriteElementString("Configuration", "Debug");
			f.WriteElementString("Platform", cfg.fixed_cpu());
			f.WriteEndElement(); // ProjectConfiguration

			f.WriteStartElement("ProjectConfiguration");
			f.WriteAttributeString("Include", string.Format("Release|{0}", cfg.fixed_cpu()));
			f.WriteElementString("Configuration", "Release");
			f.WriteElementString("Platform", cfg.fixed_cpu());
			f.WriteEndElement(); // ProjectConfiguration

			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("PropertyGroup");
			f.WriteElementString("ProjectGuid", cfg.guid);
			f.WriteElementString("DefaultLanguage", "en-us");
			f.WriteElementString("RootNamespace", "SQLitePCL.cppinterop");

			switch (cfg.env)
			{
				case "net45":
					f.WriteElementString("Keyword", "ManagedCProj");
					f.WriteElementString("TargetFrameworkVersion", "v4.5");
					break;
			}

			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(VCTargetsPath)\\Microsoft.Cpp.Default.props");
			f.WriteEndElement(); // Import

			f.WriteStartElement("PropertyGroup");
			f.WriteElementString("ConfigurationType", "DynamicLibrary");

			switch (cfg.env)
			{
				case "net45":
					f.WriteElementString("PlatformToolset", PlatformToolset);
					f.WriteElementString("CLRSupport", "true");
					break;
			}

			f.WriteEndElement(); // PropertyGroup

			switch (cfg.env)
			{
				case "net45":
					f.WriteStartElement("ItemDefinitionGroup");
					f.WriteStartElement("ClCompile");
					write_cpp_define(f, "NEED_TYPEDEFS");
					write_cpp_define(f, "WIN32");
					f.WriteEndElement(); // ClCompile
					f.WriteEndElement(); // ItemDefinitionGroup
					break;
			}

			f.WriteStartElement("PropertyGroup");
			f.WriteAttributeString("Condition", string.Format(" '$(Configuration)' == 'Debug' "));
			f.WriteElementString("UseDebugLibraries", "true");
			f.WriteElementString("LinkKeyFile", @"..\key.snk");
			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("PropertyGroup");
			f.WriteAttributeString("Condition", string.Format(" '$(Configuration)' == 'Release' "));
			f.WriteElementString("UseDebugLibraries", "false");
			f.WriteElementString("LinkKeyFile", @"..\key.snk");
			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(VCTargetsPath)\\Microsoft.Cpp.props");
			f.WriteEndElement(); // Import

			f.WriteStartElement("PropertyGroup");
			f.WriteElementString("TargetName", "SQLitePCL.cppinterop");
			f.WriteElementString("OutDir", string.Format("$(Configuration)\\bin\\{0}\\", cfg.get_dest_subpath()));
			f.WriteElementString("IntDir", string.Format("$(Configuration)\\obj\\{0}\\", cfg.get_dest_subpath()));
			write_cpp_includepath(f, root, "sqlite3\\");
			f.WriteElementString("LinkIncremental", "false");
			f.WriteElementString("GenerateManifest", "false");
			f.WriteEndElement(); // PropertyGroup

			f.WriteStartElement("ItemDefinitionGroup");
			f.WriteStartElement("ClCompile");
			f.WriteElementString("PrecompiledHeader", "NotUsing");
			f.WriteElementString("AdditionalOptions", "/bigobj %(AdditionalOptions)");
			//f.WriteElementString("CompileAsWinRT", "false");
			//f.WriteElementString("SDLCheck", "false");
			f.WriteEndElement(); // ClCompile
			f.WriteStartElement("Link");
			f.WriteElementString("SubSystem", "Console");
			f.WriteElementString("IgnoreAllDefaultLibraries", "false");
			f.WriteElementString("AdditionalDependencies", string.Format("$(Configuration)\\bin\\{0}\\sqlite3.lib;%(AdditionalDependencies)", cfg.get_sqlite3_item().get_dest_subpath()));
			//f.WriteElementString("GenerateWindowsMetadata", "false");
			f.WriteEndElement(); // Link
			f.WriteEndElement(); // ItemDefinitionGroup

			f.WriteStartElement("ItemDefinitionGroup");
			f.WriteAttributeString("Condition", string.Format("'$(Configuration)'=='{0}' ", "Debug"));
			f.WriteStartElement("ClCompile");
			f.WriteElementString("Optimization", "Disabled");
			write_cpp_define(f, "_DEBUG");
			f.WriteEndElement(); // ClCompile
			f.WriteStartElement("Link");
			f.WriteElementString("GenerateDebugInformation", "true");
			f.WriteEndElement(); // Link
			f.WriteEndElement(); // ItemDefinitionGroup

			f.WriteStartElement("ItemDefinitionGroup");
			f.WriteStartElement("ClCompile");
			f.WriteAttributeString("Condition", string.Format("'$(Configuration)'=='{0}' ", "Release"));
			f.WriteElementString("Optimization", "MaxSpeed");
			//f.WriteElementString("FunctionLevelLinking", "true");
			//f.WriteElementString("IntrinsicFunctions", "true");
			write_cpp_define(f, "NDEBUG");
			f.WriteEndElement(); // ClCompile
			f.WriteStartElement("Link");
			f.WriteElementString("GenerateDebugInformation", "false");
			//f.WriteElementString("EnableCOMDATFolding", "true");
			//f.WriteElementString("OptimizeReferences", "true");
			f.WriteEndElement(); // Link
			f.WriteEndElement(); // ItemDefinitionGroup

			f.WriteStartElement("ItemGroup");
			f.WriteStartElement("ClCompile");
			f.WriteAttributeString("Include", Path.Combine(root, "src\\cpp\\sqlite3_cx.cpp"));
			f.WriteEndElement(); // ClCompile
			f.WriteEndElement(); // ItemGroup

			switch (cfg.env)
			{
				case "net45":
					f.WriteStartElement("ItemGroup");
					f.WriteStartElement("Reference");
					f.WriteAttributeString("Include", "System");
					f.WriteEndElement(); // Reference
					f.WriteEndElement(); // ItemGroup
					break;
			}

#if not
			// TODO if this supported other ways of getting sqlite, like
			// _dynamic_sqlite3 or _static_sqlcipher, the following would be different

			f.WriteStartElement("ItemGroup");
			f.WriteStartElement("ProjectReference");
			{
				config_sqlite3 other = cfg.get_sqlite3_item();
				f.WriteAttributeString("Include", other.get_project_filename());
				f.WriteElementString("Project", other.guid);
			}
			f.WriteEndElement(); // ProjectReference
			f.WriteEndElement(); // ItemGroup
#endif

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(VCTargetsPath)\\Microsoft.Cpp.targets");
			f.WriteEndElement(); // Import

			switch (cfg.env)
			{
				case "net45":
					break;
			}

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

    private static void write_android_native_libs(string root, XmlWriter f, string which)
    {
        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\x86\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("x86\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary

        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\x86_64\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("x86_64\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary

        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\armeabi\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("armeabi\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary

        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\arm64-v8a\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("arm64-v8a\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary

        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\armeabi-v7a\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("armeabi-v7a\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary

#if not
        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\mips\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("mips\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary

        f.WriteStartElement("EmbeddedNativeLibrary");
        f.WriteAttributeString("Include", Path.Combine(root, string.Format("android\\{0}\\libs\\mips64\\libpackaged_{0}.so", which)));
        f.WriteElementString("CopyToOutputDirectory", "Always");
        f.WriteElementString("Link", string.Format("mips64\\libpackaged_{0}.so", which));
        f.WriteEndElement(); // EmbeddedNativeLibrary
#endif

    }

	private static void gen_pcl(config_pcl cfg, string root, string top)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, cfg.get_project_filename()), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			switch (cfg.env)
			{
				default:
					f.WriteAttributeString("ToolsVersion", ToolsVersion);
					break;
			}
			f.WriteAttributeString("DefaultTargets", "Build");

			switch (cfg.env)
			{
				case "wp81_sl":
					break;
				default:
					// TODO is this actually needed?
					f.WriteStartElement("Import");
					f.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props");
					f.WriteAttributeString("Condition", "Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')");
					f.WriteEndElement(); // Import
					break;
			}

			f.WriteStartElement("PropertyGroup");

			f.WriteElementString("ProjectGuid", cfg.guid);
			if (cfg.is_portable())
			{
				write_project_type_guids(f, GUID_PCL, GUID_CSHARP);
			}
			else
			{
			}

			f.WriteStartElement("Configuration");
			f.WriteAttributeString("Condition", " '$(Configuration)' == '' ");

			f.WriteString("Debug");

			f.WriteEndElement(); // Configuration

			f.WriteElementString("SchemaVersion", "2.0");
			f.WriteElementString("Platform", cfg.cpu.Replace(" ", ""));
			f.WriteElementString("DefaultLanguage", "en-us");
			//f.WriteElementString("FileAlignment", "512");
			f.WriteElementString("WarningLevel", "4");
			//f.WriteElementString("PlatformTarget", cfg.cpu.Replace(" ", ""));
			f.WriteElementString("OutputType", "Library");
			f.WriteElementString("RootNamespace", "SQLitePCL");
			f.WriteElementString("AssemblyName", "SQLitePCL.raw"); // match the name in get_products()
			f.WriteElementString("SignAssembly", "True");
			f.WriteElementString("AssemblyOriginatorKeyFile", "../key.snk");

			List<string> defines = new List<string>();

			switch (cfg.env)
			{
				case "profile158":
					f.WriteElementString("TargetFrameworkVersion", "v4.0");
					break;
				case "profile111":
				case "profile78":
				case "profile259":
					f.WriteElementString("TargetFrameworkVersion", "v4.5");
					break;
				case "net45":
					f.WriteElementString("ProductVersion", "12.0.0");
					f.WriteElementString("TargetFrameworkVersion", "v4.5");
					break;
			}

			if (cfg.is_pinvoke())
			{
				switch (cfg.what)
				{
					case "packaged_sqlite3":
						if (cfg.env == "android")
						{
							defines.Add("PINVOKE_FROM_PACKAGED_SQLITE3");
						}
                        else if (cfg.env == "net45")
						{
							defines.Add("PINVOKE_FROM_PACKAGED_SQLITE3");
						}
						else if (cfg.env == "unified")
						{
							defines.Add("PINVOKE_FROM_INTERNAL_SQLITE3");
						}
						else if (cfg.env == "ios")
						{
							defines.Add("PINVOKE_FROM_INTERNAL_SQLITE3");
						}
						break;
					case "packaged_sqlcipher":
						if (cfg.env == "android")
						{
							defines.Add("PINVOKE_FROM_PACKAGED_SQLCIPHER");
						}
                        else if (cfg.env == "net45")
						{
							defines.Add("PINVOKE_FROM_PACKAGED_SQLCIPHER");
						}
						else if (cfg.env == "unified")
						{
							defines.Add("PINVOKE_FROM_INTERNAL_SQLCIPHER");
						}
						else if (cfg.env == "ios")
						{
							defines.Add("PINVOKE_FROM_INTERNAL_SQLCIPHER");
						}
						break;
					case "sqlite3":
					default:
						if (cfg.cpu == "anycpu") {
							switch (cfg.env)
							{
#if not
// TODO I wish these would work.  They don't.  We can't build
// sqlite3.dll without a dependency on the VC++ runtime, which
// for RT platforms is only available as an extenson SDK, which
// refuses to build with AnyCPU.
								case "winrt80":
								case "winrt81":
									defines.Add("PINVOKE_ANYCPU_WINRT");
									break;
								case "wp81_rt":
									defines.Add("PINVOKE_ANYCPU_WP8");
									break;
#endif
								case "net45":
									defines.Add("PINVOKE_ANYCPU_NET45");
									break;
								default:
                                                                        // TODO are there any situations where this will work? 
									defines.Add("PINVOKE_FROM_SQLITE3");
									break;
							}
						}
						else {
							defines.Add("PINVOKE_FROM_SQLITE3");
						}
						break;
				}
			}

			if (cfg.is_portable())
			{
				f.WriteElementString("TargetFrameworkProfile", cfg.env);
			}

			f.WriteEndElement(); // PropertyGroup

			write_section(cfg, f, true, defines);
			write_section(cfg, f, false, defines);

			f.WriteStartElement("ItemGroup");
			switch (cfg.env)
			{
				case "ios":
				case "unified":
				case "android":
				case "net45":
					write_reference(f, "System");
					write_reference(f, "System.Core");
					break;
			}
			switch (cfg.env)
			{
				case "unified":
					write_reference(f, "Xamarin.iOS");
					break;
				case "ios":
					write_reference(f, "monotouch");
					break;
				case "android":
					write_reference(f, "Mono.Android");
					break;
			}
			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("ItemGroup");
			write_cs_compile(f, root, "src\\cs\\AssemblyInfo.cs");
			write_cs_compile(f, root, "src\\cs\\raw.cs");
			write_cs_compile(f, root, "src\\cs\\intptrs.cs");
			write_cs_compile(f, root, "src\\cs\\isqlite3.cs");
			f.WriteEndElement(); // ItemGroup

			if (cfg.is_portable())
			{
				f.WriteStartElement("ItemGroup");
				write_cs_compile(f, root, "src\\cs\\sqlite3_bait.cs");
				f.WriteEndElement(); // ItemGroup
			}
			else if (cfg.is_cppinterop())
			{
				f.WriteStartElement("ItemGroup");
				write_cs_compile(f, root, "src\\cs\\sqlite3_cppinterop.cs");
				write_cs_compile(f, root, "src\\cs\\util.cs");
				f.WriteEndElement(); // ItemGroup
			}
			else if (cfg.is_pinvoke())
			{
				f.WriteStartElement("ItemGroup");
				write_cs_compile(f, root, "src\\cs\\sqlite3_pinvoke.cs");
				write_cs_compile(f, root, "src\\cs\\util.cs");
				f.WriteEndElement(); // ItemGroup
			}

			if (cfg.is_cppinterop())
			{
				f.WriteStartElement("ItemGroup");
				f.WriteStartElement("ProjectReference");
				{
					config_cppinterop other = cfg.get_cppinterop_item();
					f.WriteAttributeString("Include", other.get_project_filename());
					f.WriteElementString("Project", other.guid);
					f.WriteElementString("Name", other.get_name());
					//f.WriteElementString("Private", "true");
				}
				f.WriteEndElement(); // ProjectReference
				f.WriteEndElement(); // ItemGroup
			}

			if (cfg.is_portable())
			{
				f.WriteStartElement("Import");
				f.WriteAttributeString("Project", "$(MSBuildExtensionsPath32)\\Microsoft\\Portable\\$(TargetFrameworkVersion)\\Microsoft.Portable.CSharp.targets");
				f.WriteEndElement(); // Import
			}
			else
			{
				switch (cfg.env)
				{
				}

				switch (cfg.env)
				{
					case "unified":
						f.WriteStartElement("Import");
						f.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\\Xamarin\\iOS\\Xamarin.iOS.CSharp.targets");
						f.WriteEndElement(); // Import

                        if (cfg.what == "packaged_sqlite3")
                        {
                            f.WriteStartElement("ItemGroup");
                            // TODO warning says this is deprecated
                            f.WriteStartElement("ManifestResourceWithNoCulture");
                            f.WriteAttributeString("Include", Path.Combine(root, "apple\\libs\\ios\\packaged_sqlite3.a"));
                            f.WriteElementString("Link", "packaged_sqlite3.a");
                            f.WriteEndElement(); // ManifestResourceWithNoCulture
                            f.WriteEndElement(); // ItemGroup
                        }
                        if (cfg.what == "packaged_sqlcipher")
                        {
                            f.WriteStartElement("ItemGroup");

                            // TODO warning says this is deprecated
                            f.WriteStartElement("ManifestResourceWithNoCulture");
                            f.WriteAttributeString("Include", Path.Combine(root, "apple\\libs\\ios\\packaged_sqlcipher.a"));
                            f.WriteElementString("Link", "packaged_sqlcipher.a");
                            f.WriteEndElement(); // ManifestResourceWithNoCulture

                            // TODO warning says this is deprecated
                            f.WriteStartElement("ManifestResourceWithNoCulture");
                            f.WriteAttributeString("Include", Path.Combine(root, "apple\\libs\\ios\\libcrypto.a"));
                            f.WriteElementString("Link", "libcrypto.a");
                            f.WriteEndElement(); // ManifestResourceWithNoCulture

                            f.WriteEndElement(); // ItemGroup
                        }
						break;
					case "ios":
						f.WriteStartElement("Import");
						f.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\\Xamarin\\iOS\\Xamarin.MonoTouch.CSharp.targets");
						f.WriteEndElement(); // Import

                        if (cfg.what == "packaged_sqlite3")
                        {
                            f.WriteStartElement("ItemGroup");
                            // TODO warning says this is deprecated
                            f.WriteStartElement("ManifestResourceWithNoCulture");
                            f.WriteAttributeString("Include", Path.Combine(root, "apple\\libs\\ios\\packaged_sqlite3.a"));
                            f.WriteElementString("Link", "packaged_sqlite3.a");
                            f.WriteEndElement(); // ManifestResourceWithNoCulture
                            f.WriteEndElement(); // ItemGroup
                        }
                        if (cfg.what == "packaged_sqlcipher")
                        {
                            f.WriteStartElement("ItemGroup");

                            // TODO warning says this is deprecated
                            f.WriteStartElement("ManifestResourceWithNoCulture");
                            f.WriteAttributeString("Include", Path.Combine(root, "apple\\libs\\ios\\packaged_sqlcipher.a"));
                            f.WriteElementString("Link", "packaged_sqlcipher.a");
                            f.WriteEndElement(); // ManifestResourceWithNoCulture

                            // TODO warning says this is deprecated
                            f.WriteStartElement("ManifestResourceWithNoCulture");
                            f.WriteAttributeString("Include", Path.Combine(root, "apple\\libs\\ios\\libcrypto.a"));
                            f.WriteElementString("Link", "libcrypto.a");
                            f.WriteEndElement(); // ManifestResourceWithNoCulture

                            f.WriteEndElement(); // ItemGroup
                        }
						break;
					case "android":
						if (cfg.what == "packaged_sqlite3")
						{
                            f.WriteStartElement("ItemGroup");
                            write_android_native_libs(root, f, "sqlite3");
                            f.WriteEndElement(); // ItemGroup
						}

						if (cfg.what == "packaged_sqlcipher")
						{
                            f.WriteStartElement("ItemGroup");
                            write_android_native_libs(root, f, "sqlcipher");
                            f.WriteEndElement(); // ItemGroup
                        }

						f.WriteStartElement("Import");
						f.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\\Novell\\Novell.MonoDroid.CSharp.targets");
						f.WriteEndElement(); // Import

						break;
					case "net45":
						f.WriteStartElement("Import");
						f.WriteAttributeString("Project", "$(MSBuildToolsPath)\\Microsoft.CSharp.targets");
						f.WriteEndElement(); // Import
						break;
				}
			}

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	// TODO the following function works when cfg.env is winrt80.  it might
	// not work for any other configuration.  for now, that's fine.
	// or net45
	private static void gen_tests(config_tests cfg, string root, string top)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, cfg.get_project_filename()), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			switch (cfg.env)
			{
				default:
					f.WriteAttributeString("ToolsVersion", ToolsVersion);
					break;
			}
			f.WriteAttributeString("DefaultTargets", "Build");

			switch (cfg.env)
			{
				case "wp81_sl":
					break;
				default:
					// TODO is this actually needed?
					f.WriteStartElement("Import");
					f.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props");
					f.WriteAttributeString("Condition", "Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')");
					f.WriteEndElement(); // Import
					break;
			}

			f.WriteStartElement("PropertyGroup");

			f.WriteElementString("ProjectGuid", cfg.guid);

			switch (cfg.env)
			{
				case "net45":
					write_project_type_guids(f, GUID_TEST, GUID_CSHARP);
					break;
			}

			f.WriteStartElement("Configuration");
			f.WriteAttributeString("Condition", " '$(Configuration)' == '' ");

			f.WriteString("Debug");

			f.WriteEndElement(); // Configuration

			f.WriteElementString("SchemaVersion", "2.0");
			//f.WriteElementString("Platform", cfg.cpu.Replace(" ", ""));
			f.WriteElementString("DefaultLanguage", "en-us");
			//f.WriteElementString("FileAlignment", "512");
			f.WriteElementString("WarningLevel", "4");
			//f.WriteElementString("PlatformTarget", cfg.cpu.Replace(" ", ""));
			f.WriteElementString("OutputType", "Library");
			// TODO f.WriteElementString("RootNamespace", "SQLitePCL");
			// TODO f.WriteElementString("AssemblyName", "SQLitePCL"); // match the name in get_products()
			f.WriteElementString("SignAssembly", "True");
			f.WriteElementString("AssemblyOriginatorKeyFile", "../key.snk");


			List<string> defines = new List<string>();

			switch (cfg.env)
			{
				case "profile158":
					f.WriteElementString("TargetFrameworkVersion", "v4.0");
					break;
				case "profile111":
				case "profile78":
				case "profile259":
					f.WriteElementString("TargetFrameworkVersion", "v4.5");
					break;
				case "net45":
					f.WriteElementString("ProductVersion", "12.0.0");
					f.WriteElementString("TargetFrameworkVersion", "v4.5");
					f.WriteElementString("ReferencePath", "$(ProgramFiles)\\Common Files\\microsoft shared\\VSTT\\$(VisualStudioVersion)\\UITestExtensionPackages");
					f.WriteElementString("TestProjectType", "UnitTest");
					break;
			}

			// TODO maybe define NUNIT

			f.WriteEndElement(); // PropertyGroup

			write_section(cfg.get_dest_subpath(), f, true, defines);
			write_section(cfg.get_dest_subpath(), f, false, defines);

			f.WriteStartElement("ItemGroup");
			switch (cfg.env)
			{
				case "net45":
					write_reference(f, "System");
					write_reference(f, "System.Core");
					write_reference(f, "Microsoft.VisualStudio.QualityTools.UnitTestFramework");
					break;
			}
			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("ItemGroup");
			write_cs_compile(f, root, "src\\cs\\test_cases.cs");
			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("ItemGroup");
			f.WriteStartElement("ProjectReference");
			if (cfg.env == "net45")
			{
				var a = projects.find_pcls(
					"net45",
					"cppinterop",
					"sqlite3",
					"x86",
					"static"
					);
				config_pcl other = a[0];
				f.WriteAttributeString("Include", other.get_project_filename());
				f.WriteElementString("Project", other.guid);
				f.WriteElementString("Name", other.get_name());
				//f.WriteElementString("Private", "true");
			}
			f.WriteEndElement(); // ProjectReference
			f.WriteStartElement("ProjectReference");
			{
				config_higher other = projects.find_ugly(cfg.pcl);
				f.WriteAttributeString("Include", other.get_project_filename());
				f.WriteElementString("Project", other.guid);
				f.WriteElementString("Name", other.get_name());
				//f.WriteElementString("Private", "true");
			}
			f.WriteEndElement(); // ProjectReference
			f.WriteEndElement(); // ItemGroup

			{
				switch (cfg.env)
				{
					case "net45":
						f.WriteStartElement("Import");
						f.WriteAttributeString("Project", "$(VSToolsPath)\\TeamTest\\Microsoft.TestTools.targets");
						f.WriteAttributeString("Condition", "Exists('$(VSToolsPath)\\TeamTest\\Microsoft.TestTools.targets')");
						f.WriteEndElement(); // Import

						f.WriteStartElement("Import");
						f.WriteAttributeString("Project", "$(MSBuildToolsPath)\\Microsoft.CSharp.targets");
						f.WriteEndElement(); // Import
						break;
				}
			}

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	private static void gen_higher(
			config_higher cfg,
			string root, 
			string top
			)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, cfg.get_project_filename()), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			f.WriteAttributeString("ToolsVersion", ToolsVersion);
			f.WriteAttributeString("DefaultTargets", "Build");

			// TODO is this actually needed?
			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props");
			f.WriteAttributeString("Condition", "Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')");
			f.WriteEndElement(); // Import

			f.WriteStartElement("PropertyGroup");

			f.WriteElementString("ProjectGuid", cfg.guid);
			write_project_type_guids(f, GUID_PCL, GUID_CSHARP);

			f.WriteStartElement("Configuration");
			f.WriteAttributeString("Condition", " '$(Configuration)' == '' ");

			f.WriteString("Debug");

			f.WriteEndElement(); // Configuration

			f.WriteElementString("SchemaVersion", "2.0");
			f.WriteElementString("Platform", "AnyCPU");
			f.WriteElementString("DefaultLanguage", "en-us");
			//f.WriteElementString("FileAlignment", "512");
			f.WriteElementString("WarningLevel", "4");
			//f.WriteElementString("PlatformTarget", cfg.cpu.Replace(" ", ""));
			f.WriteElementString("OutputType", "Library");
			//f.WriteElementString("RootNamespace", "whatever"); // TODO
			f.WriteElementString("AssemblyName", cfg.assemblyname);
			f.WriteElementString("SignAssembly", "True");
			f.WriteElementString("AssemblyOriginatorKeyFile", "../key.snk");


			switch (cfg.env)
			{
				case "profile158":
					f.WriteElementString("TargetFrameworkVersion", "v4.0");
					break;
				case "profile111":
				case "profile78":
				case "profile259":
					f.WriteElementString("TargetFrameworkVersion", "v4.5");
					break;
			}

			f.WriteElementString("TargetFrameworkProfile", cfg.env);

			f.WriteEndElement(); // PropertyGroup

			write_section(cfg.get_dest_subpath(), f, true, cfg.defines);
			write_section(cfg.get_dest_subpath(), f, false, cfg.defines);

			f.WriteStartElement("ItemGroup");
			foreach (string csfile in cfg.csfiles)
			{
				write_cs_compile(f, root, csfile);
			}
			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("ItemGroup");
			f.WriteStartElement("ProjectReference");
			{
				config_pcl other = projects.find_bait(cfg.env);
				f.WriteAttributeString("Include", other.get_project_filename());
				f.WriteElementString("Project", other.guid);
				f.WriteElementString("Name", other.get_name());
				//f.WriteElementString("Private", "true");
			}
			f.WriteEndElement(); // ProjectReference
			f.WriteEndElement(); // ItemGroup

			f.WriteStartElement("Import");
			f.WriteAttributeString("Project", "$(MSBuildExtensionsPath32)\\Microsoft\\Portable\\$(TargetFrameworkVersion)\\Microsoft.Portable.CSharp.targets");
			f.WriteEndElement(); // Import

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	public static string write_folder(StreamWriter f, string name)
	{
		string folder_guid = "{" + Guid.NewGuid().ToString().ToUpper() + "}";
		f.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
				GUID_FOLDER,
				name,
				name,
				folder_guid
				);
		f.WriteLine("EndProject");
		return folder_guid;
	}

	public static void gen_solution(string top)
	{
		using (StreamWriter f = new StreamWriter(Path.Combine(top, "sqlitepcl.sln")))
		{
			f.WriteLine("Microsoft Visual Studio Solution File, Format Version 12.00");
			f.WriteLine("# Visual Studio 2013");
			f.WriteLine("VisualStudioVersion = 12.0");
			f.WriteLine("MinimumVisualStudioVersion = 12.0");

			// solution folders

			string folder_sqlite3 = write_folder(f, "sqlite3");
			string folder_cppinterop = write_folder(f, "cppinterop");
			string folder_platforms = write_folder(f, "platforms");
			string folder_portable = write_folder(f, "portable");

			foreach (config_sqlite3 cfg in projects.items_sqlite3)
			{
				f.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
						GUID_CPP,
						cfg.get_name(),
						cfg.get_project_filename(),
						cfg.guid
						);
				f.WriteLine("EndProject");
			}

			foreach (config_cppinterop cfg in projects.items_cppinterop)
			{
				f.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
						GUID_CPP,
						cfg.get_name(),
						cfg.get_project_filename(),
						cfg.guid
						);
				f.WriteLine("\tProjectSection(ProjectDependencies) = postProject");
				config_sqlite3 other = cfg.get_sqlite3_item();
				f.WriteLine("\t\t{0} = {0}", other.guid);
				f.WriteLine("\tEndProjectSection");

				f.WriteLine("EndProject");
			}

			foreach (config_pcl cfg in projects.items_pcl)
			{
				f.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
						GUID_CSHARP,
						cfg.get_name(),
						cfg.get_project_filename(),
						cfg.guid
						);
				if (cfg.is_cppinterop())
				{
					f.WriteLine("\tProjectSection(ProjectDependencies) = postProject");
					config_cppinterop other = cfg.get_cppinterop_item();
					f.WriteLine("\t\t{0} = {0}", other.guid);
					f.WriteLine("\tEndProjectSection");
				}
				f.WriteLine("EndProject");
			}

			foreach (config_higher cfg in projects.items_higher)
			{
				f.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
						GUID_CSHARP,
						cfg.get_name(),
						cfg.get_project_filename(),
						cfg.guid
						);
				f.WriteLine("\tProjectSection(ProjectDependencies) = postProject");
				config_pcl other = projects.find_bait(cfg.env);
				f.WriteLine("\t\t{0} = {0}", other.guid);
				f.WriteLine("\tEndProjectSection");
				f.WriteLine("EndProject");
			}

			foreach (config_tests cfg in projects.items_tests)
			{
				f.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
						GUID_CSHARP,
						cfg.get_name(),
						cfg.get_project_filename(),
						cfg.guid
						);
				f.WriteLine("\tProjectSection(ProjectDependencies) = postProject");
				config_pcl other = projects.find_bait(cfg.pcl);
				f.WriteLine("\t\t{0} = {0}", other.guid);
				f.WriteLine("\tEndProjectSection");
				f.WriteLine("EndProject");
			}

			f.WriteLine("Global");

			f.WriteLine("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
			f.WriteLine("\t\tDebug|Mixed Platforms = Debug|Mixed Platforms");
			f.WriteLine("\t\tRelease|Mixed Platforms = Release|Mixed Platforms");
			f.WriteLine("\tEndGlobalSection");

			f.WriteLine("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
			foreach (config_sqlite3 cfg in projects.items_sqlite3)
			{
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.ActiveCfg = Debug|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.Build.0 = Debug|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.ActiveCfg = Release|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.Build.0 = Release|{1}", cfg.guid, cfg.fixed_cpu());
			}
			foreach (config_cppinterop cfg in projects.items_cppinterop)
			{
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.ActiveCfg = Debug|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.Build.0 = Debug|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.ActiveCfg = Release|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.Build.0 = Release|{1}", cfg.guid, cfg.fixed_cpu());
			}
			foreach (config_pcl cfg in projects.items_pcl)
			{
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.ActiveCfg = Debug|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.Build.0 = Debug|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.ActiveCfg = Release|{1}", cfg.guid, cfg.fixed_cpu());
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.Build.0 = Release|{1}", cfg.guid, cfg.fixed_cpu());
			}
			foreach (config_higher cfg in projects.items_higher)
			{
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.ActiveCfg = Debug|{1}", cfg.guid, "Any CPU");
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.Build.0 = Debug|{1}", cfg.guid, "Any CPU");
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.ActiveCfg = Release|{1}", cfg.guid, "Any CPU");
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.Build.0 = Release|{1}", cfg.guid, "Any CPU");
			}
			foreach (config_tests cfg in projects.items_tests)
			{
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.ActiveCfg = Debug|{1}", cfg.guid, "Any CPU");
				f.WriteLine("\t\t{0}.Debug|Mixed Platforms.Build.0 = Debug|{1}", cfg.guid, "Any CPU");
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.ActiveCfg = Release|{1}", cfg.guid, "Any CPU");
				f.WriteLine("\t\t{0}.Release|Mixed Platforms.Build.0 = Release|{1}", cfg.guid, "Any CPU");
			}
			f.WriteLine("\tEndGlobalSection");

			f.WriteLine("\tGlobalSection(SolutionProperties) = preSolution");
			f.WriteLine("\t\tHideSolutionNode = FALSE");
			f.WriteLine("\tEndGlobalSection");

			f.WriteLine("\tGlobalSection(NestedProjects) = preSolution");
			foreach (config_sqlite3 cfg in projects.items_sqlite3)
			{
				f.WriteLine("\t\t{0} = {1}", cfg.guid, folder_sqlite3);
			}
			foreach (config_cppinterop cfg in projects.items_cppinterop)
			{
				f.WriteLine("\t\t{0} = {1}", cfg.guid, folder_cppinterop);
			}
			foreach (config_pcl cfg in projects.items_pcl)
			{
				if (cfg.is_portable())
				{
					f.WriteLine("\t\t{0} = {1}", cfg.guid, folder_portable);
				}
				else
				{
					f.WriteLine("\t\t{0} = {1}", cfg.guid, folder_platforms);
				}
			}
			f.WriteLine("\tEndGlobalSection");

			f.WriteLine("EndGlobal");
		}
	}

	private static void write_nuspec_file_entry(config_pcl cfg, XmlWriter f, string where)
	{
		f.WriteComment(string.Format("{0}", cfg.get_name()));
		var a = new List<string>();
		cfg.get_products(a);

		foreach (string s in a)
		{
			f.WriteStartElement("file");
			f.WriteAttributeString("src", string.Format("release\\bin\\{0}", s));
			f.WriteAttributeString("target", cfg.get_nuget_target_path(where));
			f.WriteEndElement(); // file
		}
	}

	private static void write_nuspec_file_entry(config_higher cfg, XmlWriter f)
	{
		f.WriteComment(string.Format("{0}", cfg.get_name()));
		var a = new List<string>();
		cfg.get_products(a);

		foreach (string s in a)
		{
			f.WriteStartElement("file");
			f.WriteAttributeString("src", string.Format("release\\bin\\{0}", s));
			f.WriteAttributeString("target", cfg.get_nuget_target_path());
			f.WriteEndElement(); // file
		}
	}

	private static void write_nuspec_file_entry(config_sqlite3 cfg, XmlWriter f)
	{
		f.WriteComment(string.Format("{0}", cfg.get_name()));
		var a = new List<string>();
		cfg.get_products(a);

		foreach (string s in a)
		{
			f.WriteStartElement("file");
			f.WriteAttributeString("src", string.Format("release\\bin\\{0}", s));
			f.WriteAttributeString("target", cfg.get_nuget_target_path());
			f.WriteEndElement(); // file
		}
	}

	private static void write_nuspec_file_entries(XmlWriter f, string where, List<config_pcl> a)
	{
		foreach (config_pcl cfg in a)
		{
			write_nuspec_file_entry(cfg, f, where);
		}
	}

	private static void write_cppinterop_with_targets_file(XmlWriter f, List<config_pcl> a, string env, string top, bool needy, string id)
	{
		f.WriteComment(string.Format("platform assemblies for {0}", env));

		write_nuspec_file_entries(
				f, 
				"build", 
				a
				);

		f.WriteComment("empty directory in lib to avoid nuget adding a reference to the bait");

		Directory.CreateDirectory(Path.Combine(Path.Combine(top, "empty"), config_pcl.get_nuget_framework_name(env)));

		f.WriteStartElement("file");
		f.WriteAttributeString("src", string.Format("empty\\{0}\\", config_pcl.get_nuget_framework_name(env)));
		f.WriteAttributeString("target", string.Format("lib\\{0}", config_pcl.get_nuget_framework_name(env)));
		f.WriteEndElement(); // file

		f.WriteComment("msbuild .targets file to inject reference for the right cpu");

		string tname = string.Format("{0}_{1}.targets", needy?"needy":"basic", env);
		gen_nuget_targets_cppinterop(top, tname, needy, a);

		f.WriteStartElement("file");
		f.WriteAttributeString("src", tname);
		f.WriteAttributeString("target", string.Format("build\\{0}\\{1}.targets", config_pcl.get_nuget_framework_name(env), id));
		f.WriteEndElement(); // file
	}

	private const string NUSPEC_VERSION = "0.7.2.2-GHfVS";
	private const string NUSPEC_RELEASE_NOTES = "packaged_sqlite3 for Mac.";

	private static void gen_nuspec_basic(string top, string root, string id)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, string.Format("{0}.nuspec", id)), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("package", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

			f.WriteStartElement("metadata");
			f.WriteAttributeString("minClientVersion", "2.8.1");

			f.WriteElementString("id", id);
			f.WriteElementString("version", NUSPEC_VERSION);
			f.WriteElementString("title", "SQLitePCL.raw");
			f.WriteElementString("description", "This NuGet package for SQLitePCL.raw includes the configurations that are likely to Just Work for most use cases.  Specifically, for iOS and Android, this package uses the SQLite library which is built-in to the mobile OS.  For all of the Windows platforms, this package bundles an instance of the SQLite library.");
			f.WriteElementString("authors", "Eric Sink, et al");
			f.WriteElementString("owners", "Eric Sink");
			f.WriteElementString("copyright", "Copyright 2014-2015 Zumero, LLC");
			f.WriteElementString("requireLicenseAcceptance", "false");
			f.WriteElementString("licenseUrl", "https://raw.github.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT");
			f.WriteElementString("projectUrl", "https://github.com/ericsink/SQLitePCL.raw");
			f.WriteElementString("releaseNotes", NUSPEC_RELEASE_NOTES);
			f.WriteElementString("summary", "A Portable Class Library (PCL) for low-level (raw) access to SQLite");
			f.WriteElementString("tags", "sqlite pcl database monotouch ios monodroid android wp8 wpa");

			f.WriteEndElement(); // metadata

			f.WriteStartElement("files");

			{
				f.WriteComment("BEGIN sqlite3 libraries");
				foreach (config_sqlite3 cfg in projects.items_sqlite3)
				{
					if (cfg.dll)
					{
						write_nuspec_file_entry(
								cfg, 
								f
								);
					}
				}

				f.WriteComment("special case Mac packaged_sqlite3 dylib");
                f.WriteStartElement("file");
                f.WriteAttributeString("src", Path.Combine(root, "apple\\libs\\mac\\libpackaged_sqlite3.dylib"));
                f.WriteAttributeString("target", "build\\native\\sqlite3_dynamic\\mac");
                f.WriteEndElement(); // file

				f.WriteComment("END sqlite3 libraries");
			}

			// write all the bait

			f.WriteComment("BEGIN bait assemblies");
			// TODO we don't need ALL the bait assemblies in here
			foreach (config_pcl cfg in projects.items_pcl)
			{
				if (cfg.is_portable())
				{
					write_nuspec_file_entry(
							cfg, 
							f, 
							"lib"
							);
				}
			}
			f.WriteComment("END bait assemblies");

			f.WriteComment("BEGIN platform assemblies");

			f.WriteComment("BEGIN platform assemblies that use pinvoke in build dir");

                // --------------------------------
#if not // too big
            {
                f.WriteComment("special case packaged_sqlite3 for ios classic.  no targets file.");

                var a = projects.find_pcls(
                    "ios",
                    "pinvoke",
                    "packaged_sqlite3",
                    "anycpu",
                    null
                    );
                write_nuspec_file_entries(f, "build",
                        a
                        );

            }
#endif

                // --------------------------------
#if not // too big
            {
                f.WriteComment("special case packaged_sqlcipher for ios classic.  no targets file.");

                var a = projects.find_pcls(
                    "ios",
                    "pinvoke",
                    "packaged_sqlcipher",
                    "anycpu",
                    null
                    );
                write_nuspec_file_entries(f, "build",
                        a
                        );

            }
#endif

                // --------------------------------
            {
                var a = projects.find_pcls(
                    "net45",
                    "pinvoke",
                    "packaged_sqlite3",
                    "anycpu",
                    null
                    );
                write_nuspec_file_entries(f, "build",
                        a
                        );

#if not // TODO a targets file here would be helpful for Xam studio mac stuff.  but then all of net45 needs to move from lib to build.
                string tname = string.Format("{0}.targets", "unified");

		f.WriteComment("empty directory in lib to avoid nuget adding a reference to the bait");

		Directory.CreateDirectory(Path.Combine(Path.Combine(top, "empty"), config_pcl.get_nuget_framework_name("unified")));

		f.WriteStartElement("file");
		f.WriteAttributeString("src", string.Format("empty\\{0}\\", config_pcl.get_nuget_framework_name("unified")));
		f.WriteAttributeString("target", string.Format("lib\\{0}", config_pcl.get_nuget_framework_name("unified")));
		f.WriteEndElement(); // file

                gen_nuget_targets_android_or_unified_packaged_sqlite3(top, tname, a);

                f.WriteStartElement("file");
                f.WriteAttributeString("src", tname);
                f.WriteAttributeString("target", string.Format("build\\{0}\\{1}.targets", config_pcl.get_nuget_framework_name("unified"), id));
                f.WriteEndElement(); // file
#endif
            }

			f.WriteComment("END platform assemblies that use pinvoke in build dir");

			f.WriteComment("BEGIN platform assemblies that use pinvoke");
			Dictionary<string, string> pcl_env_pinvoke = new Dictionary<string, string>();
			pcl_env_pinvoke["net45"] = null;

			foreach (string env in pcl_env_pinvoke.Keys)
			{
				write_nuspec_file_entries(f, "lib",
						projects.find_pcls(
							env,
							"pinvoke",
							"sqlite3",
							"anycpu",
							null
							)
						);

				string tname = string.Format("{0}.targets", env);

				if (env == "net45")
				{
                    // TODO verify that this targets file works when its pcl is in lib and not build
                    gen_nuget_targets_pinvoke_anycpu(top, tname, env);
				}
				else
				{
					gen_nuget_targets_sqlite3_itself(top, tname, env);
				}

				f.WriteStartElement("file");
				f.WriteAttributeString("src", tname);
				f.WriteAttributeString("target", string.Format("build\\{0}\\{1}.targets", config_pcl.get_nuget_framework_name(env), id));
				f.WriteEndElement(); // file
			}
			f.WriteComment("END platform assemblies that use pinvoke");

			// TODO remove this directory first?
			Directory.CreateDirectory(Path.Combine(top, "empty"));
			
			f.WriteComment("END platform assemblies");

			f.WriteEndElement(); // files

			f.WriteEndElement(); // package

			f.WriteEndDocument();
		}
	}

	private static void gen_nuspec_ugly(string top)
	{
		string id = "SQLitePCL.ugly";

		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, string.Format("{0}.nuspec", id)), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("package", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

			f.WriteStartElement("metadata");
			f.WriteAttributeString("minClientVersion", "2.5"); // TODO 2.8.3 for unified

			f.WriteElementString("id", id);
			f.WriteElementString("version", NUSPEC_VERSION);
			f.WriteElementString("title", "SQLitePCL.ugly");
			f.WriteElementString("description", "These extension methods for SQLitePCL.raw provide a more usable API while remaining stylistically similar to the sqlite3 C API, which most C# developers would consider 'ugly'.  This package exists for people who (1) really like the sqlite3 C API, and (2) really like C#.  So far, evidence suggests that 100% of the people matching both criteria are named Eric Sink, but this package is available just in case he is not the only one of his kind.");
			f.WriteElementString("authors", "Eric Sink");
			f.WriteElementString("owners", "Eric Sink");
			f.WriteElementString("copyright", "Copyright 2014-2015 Zumero, LLC");
			f.WriteElementString("requireLicenseAcceptance", "false");
			f.WriteElementString("licenseUrl", "https://raw.github.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT");
			f.WriteElementString("projectUrl", "https://github.com/ericsink/SQLitePCL.raw");
			f.WriteElementString("releaseNotes", NUSPEC_RELEASE_NOTES);
			f.WriteElementString("summary", "Extension methods for SQLitePCL.raw, providing an ugly-but-usable API");
			f.WriteElementString("tags", "sqlite pcl database monotouch ios monodroid android wp8 wpa");

			f.WriteStartElement("dependencies");
			f.WriteStartElement("dependency");
			f.WriteAttributeString("id", "SQLitePCL.raw_basic"); // TODO change to SQLitePCL.raw (without _basic)
			f.WriteEndElement(); // dependency
			f.WriteEndElement(); // dependencies

			f.WriteEndElement(); // metadata

			f.WriteStartElement("files");

			foreach (config_higher cfg in projects.items_higher)
			{
				if (cfg.name == "ugly")
				{
					write_nuspec_file_entry(
							cfg, 
							f
							);
				}
			}

			f.WriteEndElement(); // files

			f.WriteEndElement(); // package

			f.WriteEndDocument();
		}
	}

	private static void gen_nuspec_tests(string top, string root)
	{
		string id = "SQLitePCL.tests";

		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, string.Format("{0}.nuspec", id)), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("package", "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

			f.WriteStartElement("metadata");
			f.WriteAttributeString("minClientVersion", "2.5"); // TODO 2.8.3 for the unified stuff

			f.WriteElementString("id", id);
			f.WriteElementString("version", NUSPEC_VERSION);
			f.WriteElementString("title", "Test cases for SQLitePCL.raw");
			f.WriteElementString("description", "Create a new unit test project.  Add this NuGetPackage.  Build.");
			f.WriteElementString("authors", "Eric Sink");
			f.WriteElementString("owners", "Eric Sink");
			f.WriteElementString("copyright", "Copyright 2014-2015 Zumero, LLC");
			f.WriteElementString("requireLicenseAcceptance", "false");
			f.WriteElementString("licenseUrl", "https://raw.github.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT");
			f.WriteElementString("projectUrl", "https://github.com/ericsink/SQLitePCL.raw");
			f.WriteElementString("releaseNotes", NUSPEC_RELEASE_NOTES);
			f.WriteElementString("summary", "test_cases.cs is a bunch unit tests for SQLitePCL.raw");
			f.WriteElementString("tags", "sqlite pcl database monotouch ios monodroid android wp8 wpa");

			f.WriteStartElement("dependencies");
			f.WriteStartElement("dependency");
			f.WriteAttributeString("id", "SQLitePCL.ugly");
			f.WriteEndElement(); // dependency
			f.WriteEndElement(); // dependencies

			f.WriteEndElement(); // metadata

			f.WriteStartElement("files");

			f.WriteStartElement("file");
			f.WriteAttributeString("src", Path.Combine(root, "src\\cs\\test_cases.cs"));
			f.WriteAttributeString("target", "content");
			f.WriteEndElement(); // file

			f.WriteEndElement(); // files

			f.WriteEndElement(); // package

			f.WriteEndDocument();
		}
	}

	private static void gen_nuget_targets_sqlite3_itself(string top, string tname, string env)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		// TODO should we put the cpu check code here, like the original version of this function (below)?

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, tname), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			f.WriteAttributeString("ToolsVersion", ToolsVersion);

			f.WriteStartElement("Target");
			f.WriteAttributeString("Name", string.Format("InjectReference_{0}", Guid.NewGuid().ToString()));
			f.WriteAttributeString("BeforeTargets", "ResolveAssemblyReferences");
			
			foreach (config_sqlite3 cfg in projects.items_sqlite3)
			{
				if (cfg.env != (env=="net45"?"winxp":env))
				{
					continue;
				}

				if (!cfg.dll)
				{
					continue;
				}

				bool b_platform_condition = true;

				f.WriteStartElement("ItemGroup");
				if (b_platform_condition)
				{
					// TODO put a condition flag here allowing this to be skipped
					f.WriteAttributeString("Condition", string.Format(" '$(Platform.ToLower())' == '{0}' ", cfg.cpu.ToLower()));
				}

				f.WriteStartElement("Content");
				// TODO call cfg.get_products() instead of hard-coding the sqlite3.dll name here
				f.WriteAttributeString("Include", string.Format("$(MSBuildThisFileDirectory)..\\..\\{0}", Path.Combine(cfg.get_nuget_target_path(), "sqlite3.dll")));
				// TODO link
				// TODO condition/exists ?
				f.WriteElementString("CopyToOutputDirectory", "PreserveNewest");
				f.WriteEndElement(); // Content

				f.WriteEndElement(); // ItemGroup
			}

			f.WriteEndElement(); // Target

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	private static void gen_nuget_targets_pinvoke_anycpu(string top, string tname, string env)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, tname), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			f.WriteAttributeString("ToolsVersion", ToolsVersion);

			f.WriteStartElement("Target");
			f.WriteAttributeString("Name", string.Format("InjectReference_{0}", Guid.NewGuid().ToString()));
			f.WriteAttributeString("BeforeTargets", "ResolveAssemblyReferences");
			
			f.WriteStartElement("ItemGroup");
			f.WriteAttributeString("Condition", " '$(OS)' == 'Windows_NT' ");
			foreach (config_sqlite3 cfg in projects.items_sqlite3)
			{
				if (cfg.env != (env=="net45"?"winxp":env))
				{
					continue;
				}

				if (!cfg.dll)
				{
					continue;
				}

				// TODO for net45, handle more than just windows.  deal with mono on mac and linux

				f.WriteStartElement("Content");
				// TODO call cfg.get_products() instead of hard-coding the sqlite3.dll name here
				f.WriteAttributeString("Include", string.Format("$(MSBuildThisFileDirectory)..\\..\\{0}", Path.Combine(cfg.get_nuget_target_path(), "sqlite3.dll")));
				// TODO condition/exists ?
				f.WriteElementString("Link", string.Format("{0}\\sqlite3.dll", cfg.cpu.ToLower()));
				f.WriteElementString("CopyToOutputDirectory", "PreserveNewest");
				f.WriteEndElement(); // Content
			}
			f.WriteEndElement(); // ItemGroup

			f.WriteEndElement(); // Target

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	private static void gen_nuget_targets_android_or_unified_packaged_sqlite3(string top, string tname, List<config_pcl> a)
    {
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, tname), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			f.WriteAttributeString("ToolsVersion", ToolsVersion);

			f.WriteStartElement("Target");
			f.WriteAttributeString("Name", string.Format("InjectReference_{0}", Guid.NewGuid().ToString()));
			f.WriteAttributeString("BeforeTargets", "ResolveAssemblyReferences");

			foreach (config_pcl cfg in a)
			{
				f.WriteComment(string.Format("{0}", cfg.get_name()));
				f.WriteStartElement("ItemGroup");
                if (cfg.what == "packaged_sqlite3")
                {
                    f.WriteAttributeString("Condition", string.Format(" '$(UseSQLiteFrom.ToLower())' == 'packaged_sqlite3' "));
                }
                else if (cfg.what == "packaged_sqlcipher")
                {
                    f.WriteAttributeString("Condition", string.Format(" '$(UseSQLiteFrom.ToLower())' == 'packaged_sqlcipher' "));
                }
                else
                {
                    f.WriteAttributeString("Condition", string.Format(" ('$(UseSQLiteFrom.ToLower())' != 'packaged_sqlite3') AND ('$(UseSQLiteFrom.ToLower())' != 'packaged_sqlcipher') "));
                }

				f.WriteStartElement("Reference");
				// TODO should Include be the HintPath?
				// https://github.com/onovotny/WinRTTimeZones/blob/master/NuGet/WinRTTimeZones.WP8.targets
				f.WriteAttributeString("Include", "SQLitePCL.raw");

				f.WriteElementString("HintPath", string.Format("$(MSBuildThisFileDirectory){0}", Path.Combine(cfg.get_nuget_target_subpath(), "SQLitePCL.raw.dll")));

				// TODO private?

				// TODO name?

				f.WriteEndElement(); // Reference

				f.WriteEndElement(); // ItemGroup
			}

			f.WriteEndElement(); // Target

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	private static void gen_nuget_targets_cppinterop(string top, string tname, bool needy, List<config_pcl> a)
	{
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;
		settings.OmitXmlDeclaration = false;

		Dictionary<string,string> cpus = new Dictionary<string,string>();
		foreach (config_pcl cfg in a)
		{
			cpus[cfg.cpu.ToLower()] = null;
		}

		string cond = "";
		string ok = "";
		foreach (string cpu in cpus.Keys)
		{
			if (cond.Length > 0)
			{
				cond += " AND ";
				ok += " or ";
			}

			cond += string.Format("($(Platform.ToLower()) != '{0}')", cpu);
			ok += cpu;
		}

		using (XmlWriter f = XmlWriter.Create(Path.Combine(top, tname), settings))
		{
			f.WriteStartDocument();
			f.WriteComment("Automatically generated");

			f.WriteStartElement("Project", "http://schemas.microsoft.com/developer/msbuild/2003");
			f.WriteAttributeString("ToolsVersion", ToolsVersion);

			f.WriteStartElement("Target");
			f.WriteAttributeString("Name", string.Format("check_cpu_{0}", Guid.NewGuid().ToString()));
			f.WriteAttributeString("BeforeTargets", "ResolveAssemblyReferences");
			f.WriteAttributeString("Condition", string.Format(" ( {0} ) ", cond));
			f.WriteStartElement("Warning");
			f.WriteAttributeString("Text", string.Format("$(Platform) is not supported. The Platform configuration must be {0}", ok));
			f.WriteEndElement(); // Warning
			f.WriteEndElement(); // Target

			f.WriteStartElement("Target");
			f.WriteAttributeString("Name", string.Format("InjectReference_{0}", Guid.NewGuid().ToString()));
			f.WriteAttributeString("BeforeTargets", "ResolveAssemblyReferences");

			foreach (config_pcl cfg in a)
			{
			
				bool b_platform_condition = true;

				f.WriteComment(string.Format("{0}", cfg.get_name()));
				f.WriteStartElement("ItemGroup");
				if (b_platform_condition)
				{
					f.WriteAttributeString("Condition", string.Format(" '$(Platform.ToLower())' == '{0}' ", cfg.cpu.ToLower()));
				}

				f.WriteStartElement("Reference");
				// TODO should Include be the HintPath?
				// https://github.com/onovotny/WinRTTimeZones/blob/master/NuGet/WinRTTimeZones.WP8.targets
				f.WriteAttributeString("Include", "SQLitePCL.raw");

				f.WriteElementString("HintPath", string.Format("$(MSBuildThisFileDirectory){0}", Path.Combine(cfg.get_nuget_target_subpath(), "SQLitePCL.raw.dll")));

				// TODO private?

				// TODO name?

				f.WriteEndElement(); // Reference

				if (!needy)
				{
					config_sqlite3 other = cfg.get_sqlite3_item();
					if (other != null)
					{
						f.WriteStartElement("Content");
						// TODO call other.get_products() instead of hard-coding the sqlite3.dll name here
						f.WriteAttributeString("Include", string.Format("$(MSBuildThisFileDirectory)..\\..\\{0}", Path.Combine(other.get_nuget_target_path(), "sqlite3.dll")));
						// TODO link
						// TODO condition/exists ?
						f.WriteElementString("CopyToOutputDirectory", "PreserveNewest");
						f.WriteEndElement(); // Content
					}
				}

#if not
				// TODO this doesn't seem to ever work
				// TODO try something like: microsoft.vclibs.120.sdkreference by mmaitre
				// TODO or http://timheuer.com/blog/archive/2013/11/25/add-extension-sdk-dependency-to-nuget-package-reference.aspx
				switch (cfg.env)
				{
					case "winrt80":
						f.WriteStartElement("SDKReference");
						f.WriteAttributeString("Include", "Microsoft.VCLibs, Version=11.0");
						f.WriteEndElement(); // SDKReference
						break;
					case "winrt81":
					case "wp81_rt":
					case "wp81_sl":
						f.WriteStartElement("SDKReference");
						f.WriteAttributeString("Include", "Microsoft.VCLibs, Version=12.0");
						f.WriteEndElement(); // SDKReference
						break;
				}
#endif

				f.WriteEndElement(); // ItemGroup
			}

			f.WriteEndElement(); // Target

			f.WriteEndElement(); // Project

			f.WriteEndDocument();
		}
	}

	static int VSVersion = 2012;
	static string PlatformToolset = "v110";
	static string ToolsVersion = "4.0";

	public static void Main(string[] args)
	{
		if (args.Length > 0)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "-vs" && args.Length > i+1)
					int.TryParse(args[i+1], out VSVersion);
			}
		}

		switch(VSVersion) {
			case 2008:
				PlatformToolset = "v90";
				ToolsVersion = "3.5";
				break;
			case 2010:
				PlatformToolset = "v100";
				ToolsVersion = "4.0";
				break;
			case 2012:
				PlatformToolset = "v110";
				ToolsVersion = "4.0";
				break;
			case 2013:
				PlatformToolset = "v120";
				ToolsVersion = "12.0";
				break;
			case 2015:
				PlatformToolset = "v140";
				ToolsVersion = "14.0";
				break;
		}

		projects.init();

		string root = ".."; // assumes that gen_build.exe is being run from the root directory of the project
		string top = "bld";

		// --------------------------------
		// create the build directory

		Directory.CreateDirectory(top);

		// --------------------------------
		// assign all the guids

		foreach (config_sqlite3 cfg in projects.items_sqlite3)
		{
			cfg.guid = "{" + Guid.NewGuid().ToString().ToUpper() + "}";
		}

		foreach (config_cppinterop cfg in projects.items_cppinterop)
		{
			cfg.guid = "{" + Guid.NewGuid().ToString().ToUpper() + "}";
		}

		foreach (config_pcl cfg in projects.items_pcl)
		{
			cfg.guid = "{" + Guid.NewGuid().ToString().ToUpper() + "}";
		}

		foreach (config_higher cfg in projects.items_higher)
		{
			cfg.guid = "{" + Guid.NewGuid().ToString().ToUpper() + "}";
		}

		foreach (config_tests cfg in projects.items_tests)
		{
			cfg.guid = "{" + Guid.NewGuid().ToString().ToUpper() + "}";
		}

		// --------------------------------
		// generate all the project files

		foreach (config_sqlite3 cfg in projects.items_sqlite3)
		{
			gen_sqlite3(cfg, root, top);
		}

		foreach (config_cppinterop cfg in projects.items_cppinterop)
		{
			gen_cppinterop(cfg, root, top);
		}

		foreach (config_pcl cfg in projects.items_pcl)
		{
			gen_pcl(cfg, root, top);
		}

		foreach (config_higher cfg in projects.items_higher)
		{
			gen_higher(cfg, root, top);
		}

		foreach (config_tests cfg in projects.items_tests)
		{
			gen_tests(cfg, root, top);
		}

		// --------------------------------

		gen_solution(top);

		// --------------------------------

		gen_nuspec_basic(top, root, "SQLitePCL.raw_basic");
		gen_nuspec_basic(top, root, "SQLitePCL.raw");

		gen_nuspec_ugly(top);

		gen_nuspec_tests(top, root);

		using (TextWriter tw = new StreamWriter(Path.Combine(top, "build.ps1")))
		{
			tw.WriteLine("msbuild /p:Configuration=Release sqlitepcl.sln");
		}

		using (TextWriter tw = new StreamWriter(Path.Combine(top, "pack.ps1")))
		{
			tw.WriteLine("# TODO");
			tw.WriteLine(@"..\nuget\nuget pack SQLitePCL.raw.nuspec");
			tw.WriteLine(@"..\nuget\nuget pack SQLitePCL.raw_basic.nuspec");
			tw.WriteLine(@"..\nuget\nuget pack SQLitePCL.ugly.nuspec");
			tw.WriteLine(@"..\nuget\nuget pack SQLitePCL.tests.nuspec");
			tw.WriteLine("ls *.nupkg");
		}
	}
}

