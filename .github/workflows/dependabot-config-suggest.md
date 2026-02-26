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
  bash: true
safe-outputs:
  create-issue:
    title-prefix: "[dependabot-suggest] "
    labels: [dependencies, automation]
    close-older-issues: true
    max: 1
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

5. **Compose the issue body**:

   Write a well-structured Markdown issue body that contains the following sections:

   ### 📦 Detected ecosystems
   A table listing each detected ecosystem, the manifest path, and the number of dependencies found.

   ### 🔍 Analysis of the current configuration
   - If `.github/dependabot.yml` already exists: list what is already correct, what is missing, and what could be improved (e.g., missing ecosystems, missing groups, suboptimal schedule).
   - If no `.github/dependabot.yml` exists: note that no configuration was found and that one should be created from scratch.

   ### 💡 Why group Dependabot updates?
   Explain the benefits of grouping in plain language, for example:
   - Reduce the number of individual PRs opened by Dependabot (one PR per group instead of one per package).
   - Make reviews easier: a single "bump all test-framework packages" PR is faster to review than a dozen separate ones.
   - Group updates are less likely to cause merge conflicts with each other.
   - Recommended whenever a project has 10 or more dependencies in an ecosystem.

   ### 🛠️ Step-by-step instructions
   Numbered list of exactly what a maintainer must do to apply the suggestion:
   1. Create or open `.github/dependabot.yml` in the repository root.
   2. Replace its content with the ready-to-use configuration block below (or merge the new entries in if partial configuration already exists).
   3. Commit and push the change on the default branch.
   4. Verify that Dependabot is enabled in the repository **Settings → Code security → Dependabot**.
   5. Wait for the first scheduled run or trigger a manual check via **Insights → Dependency graph → Dependabot**.

   ### ✅ Ready-to-use `dependabot.yml` configuration
   A fenced YAML code block containing the complete suggested `dependabot.yml` (version 2 header included).
   - Include every detected ecosystem with the correct `directory`.
   - Add `groups:` where warranted (see grouping rules in step 4 above).
   - Add inline comments in the YAML to explain non-obvious choices (e.g., why a group was created, why `daily` was chosen over `weekly`).

   ### ⚠️ Points requiring maintainer review
   A bulleted list of items that need human judgment, for example:
   - Confirm the update schedule matches the team's capacity.
   - Check whether any private registries or authentication configuration is needed.
   - Adjust group patterns if the suggested grouping does not reflect actual dependencies.

6. **Create the issue** using the body composed above.
   - Do **not** write or modify any file in the repository. The entire output is the issue.
   - The issue title must be clear and actionable, e.g. "Dependabot configuration: suggested setup for [detected ecosystems]".
   - Close any previous issue created by this workflow (the `close-older-issues: true` setting handles this automatically).

