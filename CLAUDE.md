# Claude Code Configuration - FinanceServices SPARC Development Environment

## Project Overview

**FinanceServices** is a .NET 9.0 microservices-based financial services platform built using a modular monolith architecture. The solution follows Domain-Driven Design (DDD) principles with CQRS patterns and includes multi-tenancy support.

**Base Namespace**: `ByteLabs.FinanceServices`

## 🚨 CRITICAL: CONCURRENT EXECUTION & FILE MANAGEMENT

**ABSOLUTE RULES**:
1. ALL operations MUST be concurrent/parallel in a single message
2. **NEVER save working files, text/mds and tests to the root folder**
3. ALWAYS organize files in appropriate subdirectories following DDD layered architecture
4. **USE CLAUDE CODE'S TASK TOOL** for spawning agents concurrently, not just MCP
5. **FOLLOW DDD LAYERED ARCHITECTURE** - respect module/service boundaries and layer separation

### ⚡ GOLDEN RULE: "1 MESSAGE = ALL RELATED OPERATIONS"

**MANDATORY PATTERNS:**
- **TodoWrite**: ALWAYS batch ALL todos in ONE call (5-10+ todos minimum)
- **Task tool (Claude Code)**: ALWAYS spawn ALL agents in ONE message with full instructions
- **File operations**: ALWAYS batch ALL reads/writes/edits in ONE message
- **Bash commands**: ALWAYS batch ALL terminal operations in ONE message
- **Memory operations**: ALWAYS batch ALL memory store/retrieve in ONE message

### 🎯 CRITICAL: Claude Code Task Tool for Agent Execution

**Claude Code's Task tool is the PRIMARY way to spawn agents:**
```csharp
// ✅ CORRECT: Use Claude Code's Task tool for parallel agent execution
[Single Message]:
  Task("Research agent", "Analyze requirements and patterns...", "researcher")
  Task("Coder agent", "Implement core features...", "coder")
  Task("Tester agent", "Create comprehensive tests...", "tester")
  Task("Reviewer agent", "Review code quality...", "reviewer")
  Task("Architect agent", "Design system architecture...", "system-architect")
```

**MCP tools are ONLY for coordination setup:**
- `mcp__claude-flow__swarm_init` - Initialize coordination topology
- `mcp__claude-flow__agent_spawn` - Define agent types for coordination
- `mcp__claude-flow__task_orchestrate` - Orchestrate high-level workflows

### 📁 File Organization Rules - DDD Architecture

**NEVER save to root folder. Follow the modular monolith structure:**

#### Modules (`/modules/{ModuleName}/`)
**Modules represent domains of business value** - cohesive units that encapsulate specific business capabilities:
- **Accounting**: General ledger, chart of accounts, journal entries
- **Payables**: Vendor management, invoice processing, payment handling
- **Receivables**: Customer billing, invoice generation, payment collection

Each module follows DDD layered architecture:
```
modules/{ModuleName}/
├── src/
│   ├── Domain.Abstractions/      # Domain interfaces and contracts
│   ├── Domain/                   # Domain entities, value objects, domain services
│   ├── Domain.Context/           # EF Core DbContext and entity configurations
│   ├── Domain.Context.SqlServer/ # SQL Server migrations
│   ├── Domain.Context.PostgreSql/# PostgreSQL migrations
│   ├── Application.Abstractions/ # Application service interfaces and DTOs
│   ├── Application/              # Application services, CQRS handlers
│   ├── HttpApi/                  # REST API controllers
│   ├── HttpApi.Client/           # Typed HTTP clients
│   ├── Blazor/                   # Blazor UI components (FluentUI)
│   └── Web/                      # MVC Razor Pages (Tailwind + Flowbite)
└── test/
    ├── TestBase/                 # Shared test infrastructure
    ├── Domain.Tests/             # Domain unit tests (xUnit)
    ├── Domain.Context.Tests/     # Database integration tests
    ├── Application.Tests/        # Application service tests
    ├── HttpApi.Tests/            # API integration tests
    ├── Blazor.Tests/             # Blazor component tests
    └── Acceptance.Tests/         # End-to-end tests (Playwright)
```

#### Services (`/services/{ServiceName}/`)
**Services represent business functional groups** that aggregate modules:
- **Administration**: System admin, settings, feature management
- **Identity**: User management, authentication
- **Saas**: Tenant management, subscriptions
- **FinanceServices**: Orchestrates Accounting, Payables, Receivables

Services follow the same DDD layered pattern as modules.

#### Hosts (`/hosts/{ServiceName}/HttpApi.Host/`)
**HTTP API entry points** for services - ASP.NET Core applications.

#### Apps (`/apps/{AppName}/`)
**Client applications**:
- **ManagementPortal**: Back-office (Blazor with FluentUI)
- **PublicFacing**: Customer portal (MVC Razor with Tailwind/Flowbite)
- **TrustedPlatform**: AuthServer and trusted services

#### Gateways (`/gateways/`)
- **YARP Gateways**: WebGateway, PublicWebGateway
- **Aspire.AppHost**: .NET Aspire orchestration

#### Shared (`/shared/`)
**Cross-cutting infrastructure**:
- `Shared.Hosting.*` - Hosting configurations
- `Shared.Localization` - Localization resources
- `Shared.Settings` - Configuration management

## Architecture Overview

This project uses SPARC (Specification, Pseudocode, Architecture, Refinement, Completion) methodology with Claude-Flow orchestration for systematic Test-Driven Development.

### Architecture Flow

```
[Apps] (Blazor/Razor)
  ↓
[Gateways] (YARP routes requests)
  ↓
[Hosts] (ASP.NET Core entry points)
  ↓
[Services] (Aggregate modules)
  ↓
[Modules] (DDD bounded contexts)
  ↓
[Domain] → [Application] → [HttpApi]
```

**Example**: ManagementPortal → WebGateway → FinanceServices.HttpApi.Host → FinanceServices Service → Accounting Module → Domain Logic

### Namespace Convention

Projects use automated namespace naming from `buildprops/common.namespaces.props`:
- Base namespace: `ByteLabs.FinanceServices`
- Module namespace: `ByteLabs.FinanceServices.{ModuleName}.{Layer}`
- Example: `ByteLabs.FinanceServices.Accounting.Domain`

## SPARC Commands

### Core Commands
- `npx claude-flow sparc modes` - List available modes
- `npx claude-flow sparc run <mode> "<task>"` - Execute specific mode
- `npx claude-flow sparc tdd "<feature>"` - Run complete TDD workflow
- `npx claude-flow sparc info <mode>` - Get mode details

### Batchtools Commands
- `npx claude-flow sparc batch <modes> "<task>"` - Parallel execution
- `npx claude-flow sparc pipeline "<task>"` - Full pipeline processing
- `npx claude-flow sparc concurrent <mode> "<tasks-file>"` - Multi-task processing

### Build Commands (NUKE Build System)

**The project uses NUKE build automation, not traditional MSBuild/dotnet CLI.**

```bash
# Build the solution
bash build.sh Compile

# Run tests
bash build.sh Test

# Run tests with filter
bash build.sh Test --test-filters "FullyQualifiedName~UnitTest"

# Create NuGet packages
bash build.sh Pack

# Publish artifacts
bash build.sh Publish

# Clean build artifacts
bash build.sh Clean

# Restore NuGet packages
bash build.sh Restore

# Restore client-side libraries (libman)
bash build.sh LibManRestore

# Create EF migrations
bash build.sh CreateEfMigrations

# Generate solution diagrams
bash build.sh GenerateDiagram
bash build.sh GenerateDocumentation
```

**Test Infrastructure:**
- **Unit tests**: `*.Tests` projects (xUnit)
- **Integration tests**: `*.Domain.Context.Tests` projects
- **Acceptance tests**: `*.Acceptance.Tests` projects (Playwright)

**Artifacts Output** (centralized via `UseArtifactsOutput=true`):
- Build outputs: `buildartifacts/`
- Published artifacts: `buildartifacts/published/`
- NuGet packages: `buildartifacts/package/`

## SPARC Workflow Phases

1. **Specification** - Requirements analysis (`sparc run spec-pseudocode`)
2. **Pseudocode** - Algorithm design (`sparc run spec-pseudocode`)
3. **Architecture** - System design (`sparc run architect`)
4. **Refinement** - TDD implementation (`sparc tdd`)
5. **Completion** - Integration (`sparc run integration`)

## Code Style & Best Practices

### DDD & Architecture Principles
- **Modular Design**: Files under 500 lines, respect bounded contexts
- **DDD Layers**: Domain → Application → HttpApi (strict separation)
- **CQRS**: Commands and Queries separated in Application layer
- **Multi-tenancy**: Built-in tenant isolation support
- **Environment Safety**: Never hardcode secrets, use configuration
- **Test-First**: Write tests before implementation (TDD with xUnit)
- **Clean Architecture**: Separate concerns, dependencies point inward

### UI Technology Standards

**MVC Razor Pages** (`.cshtml`):
- **CSS Framework**: Tailwind CSS (utility-first)
- **Component Library**: Flowbite (pre-built components)
- Use for: SEO-critical pages, server-rendered forms

**Blazor** (`.razor`):
- **Component Library**: FluentUI Blazor (Microsoft.Fast.Components.FluentUI)
- Use for: Rich interactive dashboards, SPAs
- Applies to: Blazor Server and Blazor WebAssembly

**Cross-Platform** (Mobile/Desktop):
- **Framework**: Uno Platform with WinUI
- **Design System**: FluentUI
- Targets: Windows, macOS, Linux, iOS, Android, WebAssembly

**NEVER mix UI technologies**: Use FluentUI for Blazor, Tailwind/Flowbite for Razor.

### Database Support
- **SQL Server**: `*.Domain.Context.SqlServer` projects
- **PostgreSQL**: `*.Domain.Context.PostgreSql` projects
- **Migrations**: Managed per-module with `CreateEfMigrations` build target

### Code Quality Enforcement
- **StyleCop**: Configuration in `buildprops/stylecop.json`
- **ReSharper analyzers**: Configured in build
- **Code coverage**: Coverlet with ReportGenerator
- **xUnit configuration**: `buildprops/xunit.runner.json`

## 🚀 Available Agents (54 Total)

### Core Development
`coder`, `reviewer`, `tester`, `planner`, `researcher`

### Swarm Coordination
`hierarchical-coordinator`, `mesh-coordinator`, `adaptive-coordinator`, `collective-intelligence-coordinator`, `swarm-memory-manager`

### Consensus & Distributed
`byzantine-coordinator`, `raft-manager`, `gossip-coordinator`, `consensus-builder`, `crdt-synchronizer`, `quorum-manager`, `security-manager`

### Performance & Optimization
`perf-analyzer`, `performance-benchmarker`, `task-orchestrator`, `memory-coordinator`, `smart-agent`

### GitHub & Repository
`github-modes`, `pr-manager`, `code-review-swarm`, `issue-tracker`, `release-manager`, `workflow-automation`, `project-board-sync`, `repo-architect`, `multi-repo-swarm`

### SPARC Methodology
`sparc-coord`, `sparc-coder`, `specification`, `pseudocode`, `architecture`, `refinement`

### Specialized Development
`backend-dev`, `mobile-dev`, `ml-developer`, `cicd-engineer`, `api-docs`, `system-architect`, `code-analyzer`, `base-template-generator`

### Testing & Validation
`tdd-london-swarm`, `production-validator`

### Migration & Planning
`migration-planner`, `swarm-init`

## 🎯 Claude Code vs MCP Tools

### Claude Code Handles ALL EXECUTION:
- **Task tool**: Spawn and run agents concurrently for actual work
- File operations (Read, Write, Edit, MultiEdit, Glob, Grep)
- Code generation and programming
- Bash commands and system operations (NUKE build, dotnet commands)
- Implementation work
- Project navigation and analysis
- TodoWrite and task management
- Git operations
- Package management (NuGet)
- Testing and debugging (xUnit, Playwright)

### MCP Tools ONLY COORDINATE:
- Swarm initialization (topology setup)
- Agent type definitions (coordination patterns)
- Task orchestration (high-level planning)
- Memory management
- Neural features
- Performance tracking
- GitHub integration

**KEY**: MCP coordinates the strategy, Claude Code's Task tool executes with real agents.

## 🚀 Quick Setup

```bash
# Add MCP servers (Claude Flow required, others optional)
claude mcp add claude-flow npx claude-flow@alpha mcp start
claude mcp add ruv-swarm npx ruv-swarm mcp start  # Optional: Enhanced coordination
claude mcp add flow-nexus npx flow-nexus@latest mcp start  # Optional: Cloud features
```

## MCP Tool Categories

### Coordination
`swarm_init`, `agent_spawn`, `task_orchestrate`

### Monitoring
`swarm_status`, `agent_list`, `agent_metrics`, `task_status`, `task_results`

### Memory & Neural
`memory_usage`, `neural_status`, `neural_train`, `neural_patterns`

### GitHub Integration
`github_swarm`, `repo_analyze`, `pr_enhance`, `issue_triage`, `code_review`

### System
`benchmark_run`, `features_detect`, `swarm_monitor`

### Flow-Nexus MCP Tools (Optional Advanced Features)
Flow-Nexus extends MCP capabilities with 70+ cloud-based orchestration tools:

**Key MCP Tool Categories:**
- **Swarm & Agents**: `swarm_init`, `swarm_scale`, `agent_spawn`, `task_orchestrate`
- **Sandboxes**: `sandbox_create`, `sandbox_execute`, `sandbox_upload` (cloud execution)
- **Templates**: `template_list`, `template_deploy` (pre-built project templates)
- **Neural AI**: `neural_train`, `neural_patterns`, `seraphina_chat` (AI assistant)
- **GitHub**: `github_repo_analyze`, `github_pr_manage` (repository management)
- **Real-time**: `execution_stream_subscribe`, `realtime_subscribe` (live monitoring)
- **Storage**: `storage_upload`, `storage_list` (cloud file management)

**Authentication Required:**
- Register: `mcp__flow-nexus__user_register` or `npx flow-nexus@latest register`
- Login: `mcp__flow-nexus__user_login` or `npx flow-nexus@latest login`
- Access 70+ specialized MCP tools for advanced orchestration

## 🚀 Agent Execution Flow with Claude Code

### The Correct Pattern:

1. **Optional**: Use MCP tools to set up coordination topology
2. **REQUIRED**: Use Claude Code's Task tool to spawn agents that do actual work
3. **REQUIRED**: Each agent runs hooks for coordination
4. **REQUIRED**: Batch all operations in single messages
5. **REQUIRED**: Follow DDD layered architecture when creating files

### Example: Accounting Module Development

```csharp
// Single message with all agent spawning via Claude Code's Task tool
// Following DDD layered architecture for Accounting module
[Parallel Agent Execution]:
  Task("Domain Expert", "Design Accounting domain entities, value objects, aggregates in ByteLabs.FinanceServices.Accounting.Domain. Use hooks for coordination.", "system-architect")
  Task("Application Developer", "Create CQRS handlers in Accounting.Application. Coordinate via memory.", "backend-dev")
  Task("API Developer", "Build REST controllers in Accounting.HttpApi following ASP.NET Core conventions.", "backend-dev")
  Task("Database Architect", "Design EF Core DbContext and configurations in Accounting.Domain.Context. Support SQL Server and PostgreSQL.", "code-analyzer")
  Task("UI Developer", "Create FluentUI Blazor components in Accounting.Blazor for management portal.", "coder")
  Task("Test Engineer", "Write xUnit tests across all layers: Domain.Tests, Application.Tests, HttpApi.Tests.", "tester")
  Task("Integration Tester", "Create EF Core integration tests in Domain.Context.Tests.", "tester")
  Task("DevOps Engineer", "Configure NUKE build targets and Docker containers. Document in memory.", "cicd-engineer")

  // All todos batched together (following DDD workflow)
  TodoWrite { todos: [
    {content: "Design domain entities and aggregates", status: "in_progress", activeForm: "Designing domain entities"},
    {content: "Create value objects for accounting", status: "in_progress", activeForm: "Creating value objects"},
    {content: "Define domain events", status: "pending", activeForm: "Defining domain events"},
    {content: "Implement CQRS command handlers", status: "pending", activeForm: "Implementing command handlers"},
    {content: "Implement CQRS query handlers", status: "pending", activeForm: "Implementing query handlers"},
    {content: "Create EF Core DbContext", status: "pending", activeForm: "Creating DbContext"},
    {content: "Configure entity mappings", status: "pending", activeForm: "Configuring entity mappings"},
    {content: "Create REST API controllers", status: "pending", activeForm: "Creating API controllers"},
    {content: "Build FluentUI Blazor components", status: "pending", activeForm: "Building Blazor components"},
    {content: "Write domain unit tests", status: "pending", activeForm: "Writing unit tests"}
  ]}

  // All file operations following DDD structure
  Bash "mkdir -p modules/Accounting/src/{Domain,Domain.Abstractions,Domain.Context,Application,Application.Abstractions,HttpApi,Blazor}"
  Bash "mkdir -p modules/Accounting/test/{Domain.Tests,Application.Tests,HttpApi.Tests,Domain.Context.Tests}"
  Write "modules/Accounting/src/Domain/Entities/GeneralLedger.cs"
  Write "modules/Accounting/src/Domain/ValueObjects/AccountNumber.cs"
  Write "modules/Accounting/src/Domain.Context/AccountingDbContext.cs"
  Write "modules/Accounting/src/Application/Commands/CreateJournalEntryCommandHandler.cs"
  Write "modules/Accounting/src/HttpApi/Controllers/AccountingController.cs"
  Write "modules/Accounting/src/Blazor/Components/ChartOfAccounts.razor"
  Write "modules/Accounting/test/Domain.Tests/Entities/GeneralLedgerTests.cs"
```

## 📋 Agent Coordination Protocol

### Every Agent Spawned via Task Tool MUST:

**1️⃣ BEFORE Work:**
```bash
npx claude-flow@alpha hooks pre-task --description "[task]"
npx claude-flow@alpha hooks session-restore --session-id "swarm-[id]"
```

**2️⃣ DURING Work:**
```bash
npx claude-flow@alpha hooks post-edit --file "[file]" --memory-key "swarm/[agent]/[step]"
npx claude-flow@alpha hooks notify --message "[what was done]"
```

**3️⃣ AFTER Work:**
```bash
npx claude-flow@alpha hooks post-task --task-id "[task]"
npx claude-flow@alpha hooks session-end --export-metrics true
```

## 🎯 Concurrent Execution Examples

### ✅ CORRECT WORKFLOW: MCP Coordinates, Claude Code Executes

```csharp
// Step 1: MCP tools set up coordination (optional, for complex tasks)
[Single Message - Coordination Setup]:
  mcp__claude-flow__swarm_init { topology: "mesh", maxAgents: 6 }
  mcp__claude-flow__agent_spawn { type: "researcher" }
  mcp__claude-flow__agent_spawn { type: "coder" }
  mcp__claude-flow__agent_spawn { type: "tester" }

// Step 2: Claude Code Task tool spawns ACTUAL agents that do the work
[Single Message - Parallel Agent Execution]:
  // Claude Code's Task tool spawns real agents concurrently
  Task("Research agent", "Analyze API requirements and best practices. Check memory for prior decisions.", "researcher")
  Task("Coder agent", "Implement REST endpoints with authentication. Coordinate via hooks.", "coder")
  Task("Database agent", "Design and implement database schema. Store decisions in memory.", "code-analyzer")
  Task("Tester agent", "Create comprehensive test suite with 90% coverage.", "tester")
  Task("Reviewer agent", "Review code quality and security. Document findings.", "reviewer")

  // Batch ALL todos in ONE call
  TodoWrite { todos: [
    {id: "1", content: "Research API patterns", status: "in_progress", priority: "high"},
    {id: "2", content: "Design database schema", status: "in_progress", priority: "high"},
    {id: "3", content: "Implement authentication", status: "pending", priority: "high"},
    {id: "4", content: "Build REST endpoints", status: "pending", priority: "high"},
    {id: "5", content: "Write unit tests", status: "pending", priority: "medium"},
    {id: "6", content: "Integration tests", status: "pending", priority: "medium"},
    {id: "7", content: "API documentation", status: "pending", priority: "low"},
    {id: "8", content: "Performance optimization", status: "pending", priority: "low"}
  ]}

  // Parallel file operations following DDD structure
  Bash "mkdir -p modules/Payables/src/{Domain,Application,HttpApi}"
  Bash "mkdir -p modules/Payables/test/{Domain.Tests,Application.Tests}"
  Write "modules/Payables/src/Domain/Entities/Invoice.cs"
  Write "modules/Payables/src/Application/Commands/ProcessInvoiceCommandHandler.cs"
  Write "modules/Payables/src/HttpApi/Controllers/PayablesController.cs"
  Write "modules/Payables/test/Domain.Tests/Entities/InvoiceTests.cs"
  Write "docs/architecture/PayablesModule.md"
```

### ❌ WRONG (Multiple Messages):
```csharp
Message 1: mcp__claude-flow__swarm_init
Message 2: Task("agent 1")
Message 3: TodoWrite { todos: [single todo] }
Message 4: Write "file.cs"
// This breaks parallel coordination!
```

## Performance Benefits

- **84.8% SWE-Bench solve rate**
- **32.3% token reduction**
- **2.8-4.4x speed improvement**
- **27+ neural models**

## Hooks Integration

### Pre-Operation
- Auto-assign agents by file type
- Validate commands for safety
- Prepare resources automatically
- Optimize topology by complexity
- Cache searches

### Post-Operation
- Auto-format code
- Train neural patterns
- Update memory
- Analyze performance
- Track token usage

### Session Management
- Generate summaries
- Persist state
- Track metrics
- Restore context
- Export workflows

## Advanced Features (v2.0.0)

- 🚀 Automatic Topology Selection
- ⚡ Parallel Execution (2.8-4.4x speed)
- 🧠 Neural Training
- 📊 Bottleneck Analysis
- 🤖 Smart Auto-Spawning
- 🛡️ Self-Healing Workflows
- 💾 Cross-Session Memory
- 🔗 GitHub Integration

## Development Workflow

### Local Development

1. **Prerequisites**:
   - .NET 9.0 SDK (version 9.0.202 or compatible)
   - Docker Desktop (for infrastructure dependencies)

2. **Infrastructure Dependencies**:
   - Redis (distributed cache)
   - SQL Server or PostgreSQL (persistence)
   - RabbitMQ (message bus)
   - Elasticsearch (optional, for logging)

   Start infrastructure using Docker Compose:
   ```bash
   cd etc/docker
   docker-compose -f docker-compose.infrastructure.yml up -d
   ```

3. **Build and Run**:
   ```bash
   # Restore dependencies
   bash build.sh Restore

   # Build solution
   bash build.sh Compile

   # Run tests
   bash build.sh Test

   # Run specific service (example)
   cd hosts/FinanceServices/HttpApi.Host
   dotnet run
   ```

### Working with Modules

When creating or modifying modules:
- Each module is a bounded context with its own domain model
- Modules communicate via domain events (RabbitMQ) or HTTP APIs
- Module dependencies are managed through `Directory.Build.props` inheritance
- Always include both Abstractions and implementation projects to support modularity

### Important Configuration Files

- `Directory.Build.props`: Root MSBuild properties and artifact output configuration
- `Directory.Build.targets`: Build targets and import configurations
- `Directory.Packages.props`: Centralized package version management (CPM enabled)
- `global.json`: .NET SDK version pinning
- `GitVersion.yml`: Semantic versioning configuration
- `buildprops/`: Centralized MSBuild properties for analyzers, styling, target frameworks, etc.

## Deployment

### Docker

Each microservice has a Dockerfile in its host project directory.

Build Docker images:
```bash
cd build
./build-images-locally.ps1  # Faster, requires .NET SDK
# OR
./build-images.ps1          # CI/CD friendly, multi-stage builds
```

### Kubernetes

Helm charts are located in `etc/k8s/FinanceServices/charts/`.

See `etc/k8s/README.md` for detailed Kubernetes deployment instructions, including:
- Setting up HTTPS with mkcert
- Configuring ingress
- Running the solution locally with Kubernetes

### Aspire

The solution includes .NET Aspire orchestration in `gateways/Aspire.AppHost/` for local development and testing.

## Integration Tips

1. Start with basic swarm init
2. Scale agents gradually
3. Use memory for context
4. Monitor progress regularly
5. Train patterns from success
6. Enable hooks automation
7. Use GitHub tools first
8. **Follow DDD layers**: Domain → Application → HttpApi
9. **Respect bounded contexts**: Keep module concerns separate
10. **Use NUKE build**: Don't bypass build automation

## Support

- Documentation: https://github.com/ruvnet/claude-flow
- Issues: https://github.com/ruvnet/claude-flow/issues
- Flow-Nexus Platform: https://flow-nexus.ruv.io (registration required for cloud features)

---

Remember: **Claude Flow coordinates, Claude Code creates, DDD structures!**

# important-instruction-reminders
Do what has been asked; nothing more, nothing less.
NEVER create files unless they're absolutely necessary for achieving your goal.
ALWAYS prefer editing an existing file to creating a new one.
NEVER proactively create documentation files (*.md) or README files. Only create documentation files if explicitly requested by the User.
Never save working files, text/mds and tests to the root folder.
ALWAYS follow DDD layered architecture when creating module or service files.
ALWAYS use NUKE build commands (bash build.sh) instead of direct dotnet CLI.
ALWAYS respect module boundaries and namespace conventions.
