
# Contributing to Benraz Authorization Server

We welcome contributions from the community! Whether it's a bug fix, a new feature, or improvements to the documentation, we appreciate your help in improving the project.

## How to Contribute

### 1. Fork the Repository
Start by forking the repository to your own GitHub account. You can do this by clicking the "Fork" button at the top of the repository page.

### 2. Clone Your Fork
Once you've forked the repository, clone your fork to your local machine:

```bash
git clone https://github.com/your-username/benraz-authorization-server.git
cd benraz-authorization-server
```

### 3. Create a New Branch
You must always work on a separate branch, which should be created from the `on-going-dev` branch. Create a new branch for your contribution:

```bash
git checkout -b your-branch-name on-going-dev
```

Use a meaningful name for your branch, such as `fix-bug-123` or `add-new-feature`.

### 4. Make Changes
Make the necessary changes in your branch. Please ensure that:
- You follow the project's code style and conventions.
- You write tests for new features or bug fixes.

### 5. Test Your Changes
Before submitting a pull request, make sure your changes are tested. Run the following command to build and run the project:

For .NET Core:
```bash
dotnet build
dotnet test
```

For Angular (Frontend):
```bash
npm install
ng build
ng test
```

### 6. Commit and Push Your Changes
After testing, commit your changes with a descriptive message:

```bash
git commit -m "Description of your changes"
```

Push your changes to your fork:

```bash
git push origin your-branch-name
```

### 7. Submit a Pull Request
Submit a pull request (PR) from your branch back to the `on-going-dev` branch in the original repository. Please include a detailed description of your changes, why they're necessary, and reference any relevant issues.

**Note**: Only the admin has the authority to approve PRs and create PRs to the `master` branch.

## Contribution Guidelines

- Ensure your code adheres to the project's coding standards.
- Follow best practices for security and performance.
- Write clear and concise commit messages.
- Write unit tests for new functionality or bug fixes.
- Make sure to keep your branch up to date with the latest changes from the `on-going-dev` branch.

### Code of Conduct
Please note that all interactions in the project are governed by our [Code of Conduct](CODE_OF_CONDUCT.md). We expect all contributors to be respectful and constructive when working within the community.

## Need Help?

If you have any questions about contributing, feel free to reach out by opening an issue or contacting the repository maintainers.

Thank you for contributing to Benraz Authorization Server!
