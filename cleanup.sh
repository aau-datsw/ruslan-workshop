cur_branch_name="$(git symbolic-ref HEAD 2>/dev/null)"

require_clean_work_tree () {
    # Update the index
    git update-index -q --ignore-submodules --refresh
    err=0

    # Disallow unstaged changes in the working tree
    if ! git diff-files --quiet --ignore-submodules --
    then
        echo >&2 "cannot $1: you have unstaged changes."
        git diff-files --name-status -r --ignore-submodules -- >&2
        err=1
    fi

    # Disallow uncommitted changes in the index
    if ! git diff-index --cached --quiet HEAD --ignore-submodules --
    then
        echo >&2 "cannot $1: your index contains uncommitted changes."
        git diff-index --cached --name-status -r --ignore-submodules HEAD -- >&2
        err=1
    fi

    if [ $err = 1 ]
    then
        echo >&2 "Please commit or stash them."
        exit 1
    fi
}

require_clean_work_tree

# Prompt for backup branch
echo "Select branch name for storing backup"

read branch_name

# Store and push backup branch
git checkout -b $branch_name

git push --set-upstream

git checkout $cur_branch_name

# Prepare repo for next event
rm -rf services/*Service

cp -f templates/group_keys.json.template group_keys.json

python build.py -f