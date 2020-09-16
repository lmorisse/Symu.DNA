# Symu.DNA as Dynamic Network Analysis

``Develop your own application to analyse social [SNA](https://en.wikipedia.org/wiki/Social_network_analysis) and organizational [ONA](https://en.wikipedia.org/wiki/Organizational_network_analysis) networks, statically or dynamically [DNA](https://en.wikipedia.org/wiki/Dynamic_network_analysis)``<br>
``Symu.DNA`` is part of ``Symu Suite``, for organizational modeling, analysis and simulating.
``Symu.DNA`` is a core of social and organizational network analysis library for static and dynamic analysis, written in C#.
It implements agnostic organizations as social groups to target the most general use cases.

Some useful links:
* [Website : symu.org](https://symu.org/)
* [Documentation : docs.symu.org](http://docs.symu.org/)
* [Code : github.symu.org](http://github.symu.org/)
* [Issues : github.symu.org/issues](http://github.symu.org/issues/)
* [Twitter : symuorg](https://twitter.com/symuorg)

## How it works

``Symu.DNA`` models social group and organization as a meta-network which is a list of specific networks that characterizing organizational architectures such as :

* Social network
* Knowledge network
* Information network
* Capabilities network
* Skills network
* Assigment, needs, requirements, work, ....

## What it is

Meta-network is used for laying out the relation among types of those networks. 
It creates statistical and graphical (not yet available) models of the people, tasks, groups, knowledge and resources of organizational systems.
Those metrics assess and identify change within and across networks.

## Why open source

Because we believe that such a framework is valuable for organizations and academics, because there are few c # frameworks available.

### Academic program

``Symu.DNA`` is based on statistical techniques based on different theories such as graph, network, dynamic networks, ...

With our **academic program**, we will first implement algorithms that you want to use for you.

## Getting Started
The main project is [Symu.DNA](https://github.com/lmorisse/Symu.DNA/tree/master/sourceCode/SymuDNA). This is the framework you'll use to build your own application.
There isn't GUI mode yet.

### Installing


### Building

``Symu.DNA`` is built upon different repositories. We don't use git submodules. So that, to build Symu.DNA and its examples solutions, you'll need to check the dependencies manually.

#### Symu.DNA dependencies
To build Symu you have to add the Symu.Common.dll as a dependency. You find this library in the [Symu.Common](https://github.com/lmorisse/Symu.Common/releases/latest) and [Symu.DNA](https://github.com/lmorisse/Symu.DNA/releases/latest) repositories.

#### External dependencies
* [Math.net](https://www.math.net/)

### Running

As it is a core library, you can't run ``Symu.DNA`` as is.

## Contributors

See the list of [CONTRIBUTORS](CONTRIBUTORS.md) who participated in this project.

## Contributing

Please read [CONTRIBUTING](CONTRIBUTING.md) for details on how you can contribute and the process for contributing.

## Code of conduct

Please read [CODE_OF_CONDUCT](CODE_OF_CONDUCT.md) for details on our code of conduct if you want to contribute.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/lmorisse/Symu/releases). 

## License

This project is licensed under the GNU General Public License v2.0 - see the [LICENSE](LICENSE) file for details

## Support

Paid consulting and support options are available from the corporate sponsors. See [Symu services](https://symu.org/services/).

## Integration

Symu.DNA is used in projects:
- [``Symu``](http://github.symu.org/): a multi-agent system, time based with discrete events, for the co-evolution of agents and socio-cultural environments.
- [``Symu.biz``](http://symu.biz): an enterprise level implementation of ``Symu``
