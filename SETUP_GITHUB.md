# GitHub Repository Setup Instructions

This guide will walk you through creating a GitHub repository and connecting your local repository to it.

## Prerequisites

- A GitHub account
- Git installed and configured on your local machine
- Your local repository initialized (already done)

## Step 1: Create a New GitHub Repository

1. **Log in to GitHub**
   - Go to [https://github.com](https://github.com)
   - Sign in to your account

2. **Create a New Repository**
   - Click the "+" icon in the top right corner
   - Select "New repository" from the dropdown menu

3. **Configure Repository Settings**
   - **Repository name**: `RacingSimFFBTuner` (or your preferred name)
   - **Description**: "Windows desktop application for racing simulator force feedback tuning"
   - **Visibility**: Choose "Public" (for open source) or "Private" (if you prefer)
   - **DO NOT** initialize with:
     - ❌ README
     - ❌ .gitignore
     - ❌ License
   
   (These files already exist in your local repository)

4. **Click "Create repository"**

## Step 2: Connect Local Repository to GitHub

After creating the repository, GitHub will show you setup instructions. Use the "push an existing repository from the command line" option.

### Option A: Using HTTPS (Recommended for beginners)

1. **Copy the repository URL** from GitHub (it will look like):
   ```
   https://github.com/yourusername/RacingSimFFBTuner.git
   ```

2. **Add the remote** in your local repository:
   ```powershell
   git remote add origin https://github.com/yourusername/RacingSimFFBTuner.git
   ```

3. **Verify the remote was added**:
   ```powershell
   git remote -v
   ```
   You should see your repository URL listed.

### Option B: Using SSH (If you have SSH keys set up)

1. **Copy the SSH repository URL** from GitHub (it will look like):
   ```
   git@github.com:yourusername/RacingSimFFBTuner.git
   ```

2. **Add the remote**:
   ```powershell
   git remote add origin git@github.com:yourusername/RacingSimFFBTuner.git
   ```

## Step 3: Initial Commit and Push

1. **Stage all files**:
   ```powershell
   git add .
   ```

2. **Create the initial commit**:
   ```powershell
   git commit -m "Initial commit: Project setup with Git, LICENSE, README, and pre-commit hooks"
   ```

3. **Push to GitHub**:
   ```powershell
   git push -u origin main
   ```

   If you're using HTTPS and haven't set up credentials, GitHub will prompt you for authentication. You may need to:
   - Use a Personal Access Token (PAT) instead of your password
   - Or set up GitHub CLI for easier authentication

## Step 4: Verify the Setup

1. **Refresh your GitHub repository page**
   - You should see all your files (README.md, LICENSE, .gitignore, etc.)

2. **Verify the connection**:
   ```powershell
   git remote -v
   git branch -a
   ```

3. **Test pushing a change**:
   - Make a small change to README.md
   - Commit and push:
     ```powershell
     git add README.md
     git commit -m "Test commit"
     git push
     ```
   - Verify the change appears on GitHub

## Step 5: (Optional) Configure Branch Protection

For better code quality, consider setting up branch protection rules:

1. Go to your repository on GitHub
2. Click "Settings" → "Branches"
3. Add a rule for the `main` branch:
   - ✅ Require a pull request before merging
   - ✅ Require status checks to pass before merging
   - ✅ Require branches to be up to date before merging
   - ✅ Include administrators

## Troubleshooting

### Authentication Issues

If you encounter authentication problems:

1. **For HTTPS**: Use a Personal Access Token (PAT)
   - Go to GitHub Settings → Developer settings → Personal access tokens
   - Generate a new token with `repo` scope
   - Use the token as your password when prompted

2. **For SSH**: Ensure your SSH key is added to GitHub
   - Check: `ssh -T git@github.com`
   - If it fails, add your SSH key in GitHub Settings → SSH and GPG keys

### Remote Already Exists

If you get an error that the remote already exists:

```powershell
git remote remove origin
git remote add origin <your-repository-url>
```

### Push Rejected

If push is rejected due to unrelated histories:

```powershell
git pull origin main --allow-unrelated-histories
git push -u origin main
```

## Next Steps

After setting up GitHub:

1. ✅ Your code is now backed up on GitHub
2. ✅ Others can contribute to your open-source project
3. ✅ You can use GitHub Issues for bug tracking
4. ✅ You can use GitHub Actions for CI/CD (future enhancement)
5. ✅ Pre-commit hooks will ensure code quality on every commit

## Additional Resources

- [GitHub Documentation](https://docs.github.com)
- [Git Documentation](https://git-scm.com/doc)
- [GitHub CLI](https://cli.github.com) - Alternative way to interact with GitHub

