

Public Module VectoEngineVersion

	Public ReadOnly Property VersionNumber as String
        get
            return "1.4.3.1043"
        end get
    end Property

    Public ReadOnly Property FullVersion as String
        get
            return String.Format("VECTO-Engine {0}", VersionNumber)
        end get
    end Property
End Module
