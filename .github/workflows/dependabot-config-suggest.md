---
name: Suggest Dependabot Configuration
description: Weekly workflow that analyzes the repository and suggests creation or update of the Dependabot configuration based on the project type and size. Groups related package updates when there are many dependencies.
on:
  schedule:
    - cron: "weekly on monday"
  workflow_dispatch:
permissions:
  contents: read
tools:
  github:
    toolsets: [context, repos]
  edit:
  bash: true
safe-outputs:
  create-pull-request:
    title-prefix: "[dependabot-suggest] "
    labels: [dependencies, automation]
    draft: true
    if-no-changes: warn
    expires: 14
network: defaults
---

# Dependabot Configuration Suggester

You are an expert in GitHub Dependabot configuration. Your goal is to analyze this repository and propose an optimal `.github/dependabot.yml` configuration.

## Instructions

1. **Analyze the repository structure**:
   - List all files in the repository root and subdirectories using bash to understand the project layout.
   - Identify all package ecosystems present (e.g. `nuget`, `npm`, `pip`, `maven`, `docker`, `github-actions`, etc.).
   - Count the number of dependencies per ecosystem by reading the relevant manifest files:
     - NuGet: `*.csproj`, `*.fsproj`, `*.vbproj`, `Directory.Packages.props`, `packages.config`
     - npm/yarn: `package.json`
     - pip: `requirements*.txt`, `Pipfile`, `pyproject.toml`, `setup.py`
     - Maven/Gradle: `pom.xml`, `build.gradle`
     - Docker: `Dockerfile*`, `docker-compose*.yml`
     - GitHub Actions: `.github/workflows/*.yml` (check `uses:` directives)
   - Read the existing `.github/dependabot.yml` if it exists.

2. **Evaluate the current configuration** (if `.github/dependabot.yml` exists):
   - Check that all detected ecosystems are covered.
   - Check that the update schedule is appropriate for the project size and activity.
   - Check whether grouping is configured when there are many dependencies.
   - Identify missing ecosystems, suboptimal schedules, or missing groups.

3. **Determine project size**:
   - **Small project**: fewer than 10 total dependencies across all ecosystems → weekly schedule is fine; grouping is optional.
   - **Medium project**: 10–30 total dependencies → weekly schedule; consider grouping related packages (e.g., test frameworks, Azure SDK packages, ASP.NET packages, Microsoft packages).
   - **Large project**: more than 30 total dependencies → weekly schedule; grouping is strongly recommended to reduce PR noise.

4. **Generate a suggested `dependabot.yml`**:
   - Include every detected package ecosystem with the correct `directory` (where the manifest lives).
   - Use `interval: "weekly"` for all ecosystems unless the project is very active (many contributors, frequent releases), in which case use `"daily"`.
   - When the project has **10 or more** dependencies in an ecosystem, add a `groups:` section to cluster related packages. Typical groups for NuGet:
     - `microsoft-extensions`: patterns `Microsoft.Extensions.*`
     - `aspnetcore`: patterns `Microsoft.AspNetCore.*`
     - `azure-sdk`: patterns `Azure.*`
     - `test-frameworks`: patterns `MSTest*`, `xunit*`, `NUnit*`, `coverlet*`, `Microsoft.NET.Test.Sdk`
     - `development-tools`: minor and patch updates for remaining packages
   - Preserve any existing manual customizations (open PRs limits, registries, etc.) unless they are clearly wrong.
   - For GitHub Actions ecosystem (directory `/`), always add an entry if `.github/workflows/` contains workflow files.

5. **Write the changes**:
   - Use the `edit` tool to write the new or updated content to `.github/dependabot.yml`.
   - Ensure the file is valid YAML and contains a `version: 2` header.

6. **Create a pull request** with the updated `.github/dependabot.yml`.
   - The PR body must include:
     - A summary of what ecosystems were detected and their dependency counts.
     - An explanation of every change made compared to the existing configuration (or note that a new file was created).
     - If groups were added, explain which packages fall into each group and why.
     - A note asking maintainers to review and adjust the schedule or groups if needed.
