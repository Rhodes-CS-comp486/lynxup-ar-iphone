{
  description = "Python Dev Environment with Flask";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }: flake-utils.lib.eachDefaultSystem (system:
    let
      pkgs = import nixpkgs { inherit system; };
    in
    {
      devShell = pkgs.mkShell {
        name = "python-dev";

        buildInputs = with pkgs; [
          python3
          python3Packages.pip
          python3Packages.virtualenv
          python3Packages.flask
          python3Packages.firebase-admin
          firebase-tools
        ];

        nativeBuildInputs = with pkgs; [
          git
          direnv
          jq
        ];

        # Write custom config.fish to a temporary file and set XDG_CONFIG_HOME to point to it
        shellHook = ''
           export PS1="\n($name) $PS1"
        '';
      };
    });
}
