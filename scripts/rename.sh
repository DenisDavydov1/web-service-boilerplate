#!/bin/bash

set -e # Exit on error

# ARG1: directory
if [ -z "$1" ]; then
    echo "Error: provide root directory" >&2
    exit 1
fi

# ARG2: PascalCase project name
if [ -z "$2" ]; then
    echo "Error: provide project name in PascalCase" >&2
    exit 1
fi

# ARG3: camelCase project name
if [ -z "$3" ]; then
    echo "Error: provide project name in camelCase" >&2
    exit 1
fi

# ARG4: snake_case project name
if [ -z "$4" ]; then
    echo "Error: provide project name in snake_case" >&2
    exit 1
fi

# ARG5: SCREAMING_SNAKE_CASE project name
if [ -z "$5" ]; then
    echo "Error: provide project name in SCREAMING_SNAKE_CASE" >&2
    exit 1
fi

# ARG6: kebab-case project name
if [ -z "$6" ]; then
    echo "Error: provide project name in kebab-case" >&2
    exit 1
fi

directory=$1
pascal_case=$2
camel_case=$3
snake_case=$4
screaming_snake_case=$5
kebab_case=$6

bp_pascal_case="BoilerPlate"
bp_camel_case="boilerPlate"
bp_snake_case="boiler_plate"
bp_screaming_snake_case="BOILER_PLATE"
bp_kebab_case="boiler-plate"

naming_conventions=(
    "pascal_case"
    "camel_case"
    "snake_case"
    "screaming_snake_case"
    "kebab_case"
)

for convention in "${naming_conventions[@]}"; do
    bp_variable="bp_$convention"
    variable="${convention}"
    find $directory -type f -name "*${!bp_variable}*" -exec bash -c 'mv $0 $(echo $0 | sed -E "s/(.*)$1/\1$2/")' {} ${!bp_variable} ${!variable} \;
    find $directory -type d -name "*${!bp_variable}*" | sort -r --field-separator=/ --key=4,4n | xargs -I{} bash -c 'mv $0 $(echo $0 | sed -E "s/(.*)$1/\1$2/")' {} ${!bp_variable} ${!variable} \;
    find $directory -type f -exec bash -c 'LC_CTYPE=C && LANG=C && sed -i "" s:$1:$2:g $0' {} ${!bp_variable} ${!variable} \;
done
