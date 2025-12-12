using C4Sharp.Diagrams;
using Nuke.Common.ProjectModel;
using C4Sharp.Elements;
using C4Sharp.Elements.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C4Sharp.Elements.Boundaries;
using C4Sharp.Elements.Containers;
using Nuke.Common.IO;

namespace Diagrams
{
    public class SolutionDiagram : DiagramBuilder
    {
        private readonly Nuke.Common.ProjectModel.Solution _solution;
        protected override string Title { get; }
        protected override DiagramType DiagramType { get; }

        protected override IEnumerable<Structure> Structures => DiscoverStructures();
        protected override IEnumerable<Relationship> Relationships => DiscoverRelationships();

        private Dictionary<Nuke.Common.ProjectModel.Project, Structure> _projectToStructure = new Dictionary<Nuke.Common.ProjectModel.Project, Structure>();
        private Dictionary<string, Dictionary<Nuke.Common.ProjectModel.Project, Structure>> _featureGroups = new Dictionary<string, Dictionary<Nuke.Common.ProjectModel.Project, Structure>>();

        public SolutionDiagram(Nuke.Common.ProjectModel.Solution solution, string? title = null, DiagramType? diagram = null)
        {
            _solution = solution;
            Title = title ?? solution.Name;
            DiagramType = diagram ?? DiagramType.Container;
        }

        protected virtual IEnumerable<Structure> DiscoverStructures()
        {
            if (!_projectToStructure.Any())
            {
                foreach (var project in _solution.AllProjects)
                {
                    if (!string.Equals(project.GetProperty("IncludeInDiagrams"), bool.TrueString, StringComparison.InvariantCultureIgnoreCase)) continue;

                    var featureGroup = project.GetProperty("ProductFeatureGroup");


                    string defaultLablel = (project.GetProperty("AssemblyTitle") ?? project.Name).Replace(".", " ");
                    string defaultAlias = project.Name.Replace(".", "").Replace("-", "").Replace("_", "");

                    string? description = project.GetProperty("Description") ?? string.Empty;
                    IEnumerable<string> tags = Array.Empty<string>();

                    var msBuildPackageTags = project.GetProperty("PackageTags");
                    if (!string.IsNullOrWhiteSpace(msBuildPackageTags))
                    {
                        tags = msBuildPackageTags.Split(';');
                    }

                    if (project.GetProperty("OutputType") == "Exe")
                    {
                        var defaultTechnology = "Dotnet";

                        if (project.Directory.GetFiles("Dockerfile*").Any())
                        {
                            defaultTechnology = $"{defaultTechnology}, Docker Container";
                        }

                        var svc = ConstructContainer(project, defaultAlias, defaultLablel, defaultTechnology, description, tags);

                        if (!string.IsNullOrWhiteSpace(featureGroup))
                        {
                            var featureGroupStructure = _featureGroups.ContainsKey(featureGroup) ? _featureGroups[featureGroup] : new Dictionary<Nuke.Common.ProjectModel.Project, Structure>();
                            featureGroupStructure.Add(project, svc);
                            _featureGroups[featureGroup] = featureGroupStructure;
                        }
                        else
                        {
                            _projectToStructure.Add(project, svc);
                        }

                    }
                    else
                    {
                        var defaultBoundary = Boundary.Internal;

                        var structure = new Component(defaultAlias, defaultAlias);//, defaultLablel);
                        //{
                        //    Description = description,
                        //    //Technology = "C#",
                        //    Boundary = defaultBoundary,
                        //    Tags = tags
                        //};


                        if (!string.IsNullOrWhiteSpace(featureGroup))
                        {
                            var featureGroupStructure = _featureGroups.ContainsKey(featureGroup) ? _featureGroups[featureGroup] : new Dictionary<Nuke.Common.ProjectModel.Project, Structure>();
                            featureGroupStructure.Add(project, structure);
                            _featureGroups[featureGroup] = featureGroupStructure;
                        }
                        else
                        {
                            _projectToStructure.Add(project, structure);
                        }
                    }
                }
            }
            foreach (var structure in _projectToStructure)
            {
                yield return structure.Value;
            }

            if (_featureGroups.Any())
            {
                foreach (var featureGroup in _featureGroups)
                {
                    var components = new List<Component>();
                    var containers = new List<Container>();
                    foreach (var structure in featureGroup.Value)
                    {
                        if (structure.Value is Container ctr)
                        {
                            yield return ctr;
                            //containers.Add(ctr);
                        }
                        if(structure.Value is Component cmp)
                        {
                            yield return cmp;
                            //components.Add(cmp);
                        }
                    }

                    if(containers.Any())
                    {
                        yield return new SoftwareSystemBoundary($"CTR{featureGroup.Key}", featureGroup.Key, containers.ToArray());
                    }
                    if (components.Any())
                    {
                        yield return new ContainerBoundary($"CMP{featureGroup.Key}", featureGroup.Key, components.ToArray());
                    }

                }
            }
        }

        protected virtual IEnumerable<Relationship> DiscoverRelationships()
        {
            return new List<Relationship>();
        }

        private Structure ConstructContainer(Nuke.Common.ProjectModel.Project project, string alias, string label, string technology, string description, IEnumerable<string> tags)
        {
            var containerType = project.GetProperty("ContainerType") ?? "Api";

            if (string.Equals(containerType, "Api", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Api(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "SoftwareSystem", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SoftwareSystem(alias, label)
                {
                    Tags = tags ?? Array.Empty<string>(),
                    Description = description
                };
            }

            if (string.Equals(containerType, "Microservice", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Microservice(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "ShellScript", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ShellScript(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "ServerSideWebApp", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ServerSideWebApp(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "ServerlessFunction", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ServerlessFunction(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "ServerConsole", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ServerConsole(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "Queue", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Queue(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "Pipeline", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Pipeline(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "Mobile", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Mobile(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "MessageBroker", StringComparison.InvariantCultureIgnoreCase))
            {
                return new MessageBroker(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "FileSystem", StringComparison.InvariantCultureIgnoreCase))
            {
                return new FileSystem(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "EventStreaming", StringComparison.InvariantCultureIgnoreCase))
            {
                return new EventStreaming(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "Database", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Database(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "ClientSideWebApp", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ClientSideWebApp(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "ClientDesktop", StringComparison.InvariantCultureIgnoreCase))
            {
                return new ClientDesktop(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            if (string.Equals(containerType, "BlobStore", StringComparison.InvariantCultureIgnoreCase))
            {
                return new BlobStore(alias, label, technology, description)
                {
                    Tags = tags ?? Array.Empty<string>()
                };
            }

            return new Api(alias, label, technology, description)
            {
                Tags = tags ?? Array.Empty<string>(),
            };
        }
    }
}
