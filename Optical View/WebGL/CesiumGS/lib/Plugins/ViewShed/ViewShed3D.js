(function () {
    "use strict";
    function a(e) {
        return e && e.__esModule ? e : {
            default: e
        }
    }
    function n(e, t) {
        if (!(e instanceof t))
            throw new TypeError("Cannot call a class as a function")
    }
    Object.defineProperty(Cesium, "__esModule", {
        value: !0
    }),
        Cesium.ViewShed3D = void 0;
    var r = function () {
        function e(e, t) {
            for (var i = 0; i < t.length; i++) {
                var a = t[i]; a.enumerable = a.enumerable || !1, a.configurable = !0, "value" in a && (a.writable = !0),
                    Object.defineProperty(e, a.key, a)
            }
        }
        return function (t, i, a) {
            return i && e(t.prototype, i), a && e(t, a), t
        }
    }(),c = {
            cameraPosition: null,
            viewPosition: null,
            horizontalAngle: 120,
            verticalAngle: 90,
            visibleAreaColor: new Cesium.Color(0, 1, 0),
            hiddenAreaColor: new Cesium.Color(1, 0, 0),
            alpha: .5,
            distance: 100,
            frustum: !0
        };
    Cesium.ViewShed3D = function () {
        function e(t, i) {
            n(this, e), t && (i || (i = {}),
                this.viewer = t,
                this.cameraPosition = Cesium.defaultValue(i.cameraPosition, c.cameraPosition),
                this.viewPosition = Cesium.defaultValue(i.viewPosition, c.viewPosition),
                this._horizontalAngle = Cesium.defaultValue(i.horizontalAngle, c.horizontalAngle),
                this._verticalAngle = Cesium.defaultValue(i.verticalAngle, c.verticalAngle),
                this._visibleAreaColor = Cesium.defaultValue(i.visibleAreaColor, c.visibleAreaColor),
                this._hiddenAreaColor = Cesium.defaultValue(i.hiddenAreaColor, c.hiddenAreaColor),
                this._alpha = Cesium.defaultValue(i.alpha, c.alpha),
                this._distance = Cesium.defaultValue(i.distance, c.distance),
                this._frustum = Cesium.defaultValue(i.frustum, c.frustum),
                this.calback = i.calback, 
                this.cameraPosition && this.viewPosition ? (this._addToScene(), this.calback && this.calback()) : this._bindMourseEvent())
        }
        return r(e, [{
            key: "_bindMourseEvent",
            value: function () {
                var e = this,
                    t = this.viewer,
                    i = new Cesium.ScreenSpaceEventHandler(this.viewer.scene.canvas);
                i.setInputAction(function (i) {
                    var a = Cesium.getCurrentMousePosition(t.scene, i.position);
                   
                    a && (e.cameraPosition ? e.cameraPosition && !e.viewPosition && (e.viewPosition = a,
                        e._addToScene(), e._unbindMourseEvent(), e.calback && e.calback()) : e.cameraPosition = a)
                        console.log(e)
                }, Cesium.ScreenSpaceEventType.LEFT_CLICK), 
                i.setInputAction(function (i) {
                    var a = Cesium.getCurrentMousePosition(t.scene, i.endPosition);
                    if (a) {
                        var n = e.cameraPosition; n && (e.frustumQuaternion = e.getFrustumQuaternion(n, a),
                            e.distance = Number(Cesium.Cartesian3.distance(n, a).toFixed(1)))
                    }
                }, Cesium.ScreenSpaceEventType.MOUSE_MOVE),
                this._handler = i
            }
        }, {
            key: "_unbindMourseEvent",
            value: function () {
                null != this._handler && (this._handler.destroy(), delete this._handler)
            }
        }, {
            key: "_addToScene",
            value: function () {
                this.frustumQuaternion = this.getFrustumQuaternion(this.cameraPosition, this.viewPosition),
                    this._createShadowMap(this.cameraPosition, this.viewPosition),
                    this._addPostProcess(), !this.radar && this.addRadar(this.cameraPosition,this.frustumQuaternion),
                    this.viewer.scene.primitives.add(this)
            }
        }, {
            key: "_createShadowMap",
            value: function (e, t, i) {
                var a = e,
                    n = t,
                    r = this.viewer.scene,
                    o = new Cesium.Camera(r);
                o.position = a,
                    o.direction = Cesium.Cartesian3.subtract(n, a, new Cesium.Cartesian3(0, 0, 0)),
                    o.up = Cesium.Cartesian3.normalize(a, new Cesium.Cartesian3(0, 0, 0));
                var l = Number(Cesium.Cartesian3.distance(n, a).toFixed(1));
                this.distance = l,
                    o.frustum = new Cesium.PerspectiveFrustum({
                        fov: Cesium.Math.toRadians(120),
                        aspectRatio: r.canvas.clientWidth / r.canvas.clientHeight,
                        near: .1,
                        far: 5e3
                    });
                this.viewShadowMap = new Cesium.ShadowMap({
                    lightCamera: o,
                    enable: !1,
                    isPointLight: !1,
                    isSpotLight: !0,
                    cascadesEnabled: !1,
                    context: r.context,
                    pointLightRadius: l
                })
            }
        }, {
            key: "getFrustumQuaternion",
            value: function (e, t) {
                var i = Cesium.Cartesian3.normalize(Cesium.Cartesian3.subtract(t, e, new Cesium.Cartesian3), new Cesium.Cartesian3),
                    a = Cesium.Cartesian3.normalize(e, new Cesium.Cartesian3),
                    n = new Cesium.Camera(this.viewer.scene);
                n.position = e,
                    n.direction = i,
                    n.up = a,
                    i = n.directionWC,
                    a = n.upWC;
                var r = n.rightWC,
                    o = new Cesium.Cartesian3,
                    l = new Cesium.Matrix3,
                    u = new Cesium.Quaternion;
                r = Cesium.Cartesian3.negate(r, o);
                var d = l;
                return Cesium.Matrix3.setColumn(d, 0, r, d),
                    Cesium.Matrix3.setColumn(d, 1, a, d),
                    Cesium.Matrix3.setColumn(d, 2, i, d),
                    Cesium.Quaternion.fromRotationMatrix(d, u)
            }
        }, {
            key: "_addPostProcess",
            value: function () {
                var e = this,
                    i = this,
                    a = i.viewShadowMap._isPointLight ? i.viewShadowMap._pointBias : i.viewShadowMap._primitiveBias;
                this.postProcess = this.viewer.scene.postProcessStages.add(new Cesium.PostProcessStage({
                fragmentShader: `
                        uniform float czzj;
                        uniform float dis;
                        uniform float spzj;
                        uniform vec3 visibleColor;
                        uniform vec3 disVisibleColor;
                        uniform float mixNum;
                        uniform sampler2D colorTexture;
                        uniform sampler2D stcshadow; 
                        uniform sampler2D depthTexture;
                        uniform mat4 _shadowMap_matrix; 
                        uniform vec4 shadowMap_lightPositionEC; 
                        uniform vec4 shadowMap_lightDirectionEC;
                        uniform vec3 shadowMap_lightUp;
                        uniform vec3 shadowMap_lightDir;
                        uniform vec3 shadowMap_lightRight;
                        uniform vec4 shadowMap_normalOffsetScaleDistanceMaxDistanceAndDarkness; 
                        uniform vec4 shadowMap_texelSizeDepthBiasAndNormalShadingSmooth; 
                        varying vec2 v_textureCoordinates;
                        vec4 toEye(in vec2 uv, in float depth){
                            vec2 xy = vec2((uv.x * 2.0 - 1.0),(uv.y * 2.0 - 1.0));
                            vec4 posInCamera =czm_inverseProjection * vec4(xy, depth, 1.0);
                            posInCamera =posInCamera / posInCamera.w;
                            return posInCamera;
                        }
                        float getDepth(in vec4 depth){
                            float z_window = czm_unpackDepth(depth);
                            z_window = czm_reverseLogDepth(z_window);
                            float n_range = czm_depthRange.near;
                            float f_range = czm_depthRange.far;
                            return (2.0 * z_window - n_range - f_range) / (f_range - n_range);
                        }
                        float _czm_sampleShadowMap(sampler2D shadowMap, vec2 uv){
                            return texture2D(shadowMap, uv).r;
                        }
                        float _czm_shadowDepthCompare(sampler2D shadowMap, vec2 uv, float depth){
                            return step(depth, _czm_sampleShadowMap(shadowMap, uv));
                        }
                        float _czm_shadowVisibility(sampler2D shadowMap, czm_shadowParameters shadowParameters){
                            float depthBias = shadowParameters.depthBias;
                            float depth = shadowParameters.depth;
                            float nDotL = shadowParameters.nDotL;
                            float normalShadingSmooth = shadowParameters.normalShadingSmooth;
                            float darkness = shadowParameters.darkness;
                            vec2 uv = shadowParameters.texCoords;
                            depth -= depthBias;
                            vec2 texelStepSize = shadowParameters.texelStepSize;
                            float radius = 1.0;
                            float dx0 = -texelStepSize.x * radius;
                            float dy0 = -texelStepSize.y * radius;
                            float dx1 = texelStepSize.x * radius;
                            float dy1 = texelStepSize.y * radius;
                            float visibility = 
                            (
                            _czm_shadowDepthCompare(shadowMap, uv, depth)
                            +_czm_shadowDepthCompare(shadowMap, uv + vec2(dx0, dy0), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(0.0, dy0), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(dx1, dy0), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(dx0, 0.0), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(dx1, 0.0), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(dx0, dy1), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(0.0, dy1), depth) +
                            _czm_shadowDepthCompare(shadowMap, uv + vec2(dx1, dy1), depth)
                            ) * (1.0 / 9.0)
                            ;
                            return visibility;
                        }
                        vec3 pointProjectOnPlane(in vec3 planeNormal, in vec3 planeOrigin, in vec3 point){
                            vec3 v01 = point -planeOrigin;
                            float d = dot(planeNormal, v01) ;
                            return (point - planeNormal * d);
                        }
                        float ptm(vec3 pt){
                            return sqrt(pt.x*pt.x + pt.y*pt.y + pt.z*pt.z);
                        }
                        void main() 
                        { 
                            const float PI = 3.141592653589793;
                            vec4 color = texture2D(colorTexture, v_textureCoordinates);
                            vec4 currD = texture2D(depthTexture, v_textureCoordinates);
                            if(currD.r>=1.0){
                                gl_FragColor = color;
                                return;
                            }
                            
                            float depth = getDepth(currD);
                            vec4 positionEC = toEye(v_textureCoordinates, depth);
                            vec3 normalEC = vec3(1.0);
                            czm_shadowParameters shadowParameters; 
                            shadowParameters.texelStepSize = shadowMap_texelSizeDepthBiasAndNormalShadingSmooth.xy; 
                            shadowParameters.depthBias = shadowMap_texelSizeDepthBiasAndNormalShadingSmooth.z; 
                            shadowParameters.normalShadingSmooth = shadowMap_texelSizeDepthBiasAndNormalShadingSmooth.w; 
                            shadowParameters.darkness = shadowMap_normalOffsetScaleDistanceMaxDistanceAndDarkness.w; 
                            shadowParameters.depthBias *= max(depth * 0.01, 1.0); 
                            vec3 directionEC = normalize(positionEC.xyz - shadowMap_lightPositionEC.xyz); 
                            float nDotL = clamp(dot(normalEC, -directionEC), 0.0, 1.0); 
                            vec4 shadowPosition = _shadowMap_matrix * positionEC; 
                            shadowPosition /= shadowPosition.w; 
                            if (any(lessThan(shadowPosition.xyz, vec3(0.0))) || any(greaterThan(shadowPosition.xyz, vec3(1.0)))) 
                            { 
                                gl_FragColor = color;
                                return;
                            }

                            //坐标与视点位置距离，大于最大距离则舍弃阴影效果
                            vec4 lw = czm_inverseView*  vec4(shadowMap_lightPositionEC.xyz, 1.0);
                            vec4 vw = czm_inverseView* vec4(positionEC.xyz, 1.0);
                            if(distance(lw.xyz,vw.xyz)>dis){
                                gl_FragColor = color;
                                return;
                            }


                            //水平夹角限制
                            vec3 ptOnSP = pointProjectOnPlane(shadowMap_lightUp,lw.xyz,vw.xyz);
                            directionEC = ptOnSP - lw.xyz;
                            float directionECMO = ptm(directionEC.xyz);
                            float shadowMap_lightDirMO = ptm(shadowMap_lightDir.xyz);
                            float cosJJ = dot(directionEC,shadowMap_lightDir)/(directionECMO*shadowMap_lightDirMO);
                            float degJJ = acos(cosJJ)*(180.0 / PI);
                            degJJ = abs(degJJ);
                            if(degJJ>spzj/2.0){
                                gl_FragColor = color;
                                return;
                            }

                            //垂直夹角限制
                            vec3 ptOnCZ = pointProjectOnPlane(shadowMap_lightRight,lw.xyz,vw.xyz);
                            vec3 dirOnCZ = ptOnCZ - lw.xyz;
                            float dirOnCZMO = ptm(dirOnCZ);
                            float cosJJCZ = dot(dirOnCZ,shadowMap_lightDir)/(dirOnCZMO*shadowMap_lightDirMO);
                            float degJJCZ = acos(cosJJCZ)*(180.0 / PI);
                            degJJCZ = abs(degJJCZ);
                            if(degJJCZ>czzj/2.0){
                                gl_FragColor = color;
                                return;
                            }

                            shadowParameters.texCoords = shadowPosition.xy; 
                            shadowParameters.depth = shadowPosition.z; 
                            shadowParameters.nDotL = nDotL; 
                            float visibility = _czm_shadowVisibility(stcshadow, shadowParameters); 
                            if(visibility==1.0){
                                gl_FragColor = mix(color,vec4(visibleColor,1.0),mixNum);
                            }else{
                                if(abs(shadowPosition.z-0.0)<0.01){
                                    return;
                                }
                                gl_FragColor = mix(color,vec4(disVisibleColor,1.0),mixNum);
                            }
                        }
                `,
                    uniforms: {
                        czzj: function () {
                            return e.verticalAngle
                        },
                        dis: function () {
                            return e.distance
                        },
                        spzj: function () {
                            return e.horizontalAngle
                        },
                        visibleColor: function () {
                            return e.visibleAreaColor
                        },
                        disVisibleColor: function () {
                            return e.hiddenAreaColor
                        },
                        mixNum: function () {
                            return e.alpha
                        },
                        stcshadow: function () {
                            return i.viewShadowMap._shadowMapTexture
                        },
                        _shadowMap_matrix: function () {
                            return i.viewShadowMap._shadowMapMatrix
                        },
                        shadowMap_lightPositionEC: function () {
                            return i.viewShadowMap._lightPositionEC
                        },
                        shadowMap_lightDirectionEC: function () {
                            return i.viewShadowMap._lightDirectionEC
                        },
                        shadowMap_lightUp: function () {
                            return i.viewShadowMap._lightCamera.up
                        },
                        shadowMap_lightDir: function () {
                            return i.viewShadowMap._lightCamera.direction
                        },
                        shadowMap_lightRight: function () {
                            return i.viewShadowMap._lightCamera.right
                        },
                        shadowMap_texelSizeDepthBiasAndNormalShadingSmooth: function () {
                            var e = new Cesium.Cartesian2;
                            return e.x = 1 / i.viewShadowMap._textureSize.x,
                                e.y = 1 / i.viewShadowMap._textureSize.y,
                                Cesium.Cartesian4.fromElements(e.x, e.y, a.depthBias, a.normalShadingSmooth, this.combinedUniforms1)
                        },
                        shadowMap_normalOffsetScaleDistanceMaxDistanceAndDarkness: function () {
                            return Cesium.Cartesian4.fromElements(a.normalOffsetScale, i.viewShadowMap._distance, i.viewShadowMap.maximumDistance,
                                i.viewShadowMap._darkness, this.combinedUniforms2)
                        }
                    }
                }))
            }
        }, {
            key: "removeRadar",
            value: function () {
                this.viewer.entities.remove(this.radar)
            }
        }, {
            key: "resetRadar",
            value: function () {
                this.removeRadar(),this.addRadar(this.cameraPosition, this.frustumQuaternion)
            }
        }, {
            key: "addRadar",
            value: function (e, t) {
                var i = e, a = this;
                this.radar = this.viewer.entities.add({
                    position: i,
                    orientation: t,
                    rectangularSensor: new Cesium.RectangularSensorGraphics({
                        radius: a.distance,
                        xHalfAngle: Cesium.Math.toRadians(a.horizontalAngle / 2),
                        yHalfAngle: Cesium.Math.toRadians(a.verticalAngle / 2),
                        material: new Cesium.Color(0, 1, 1, .4),
                        lineColor: new Cesium.Color(1, 1, 1, 1),
                        slice: 8,
                        showScanPlane: !1,
                        scanPlaneColor: new Cesium.Color(0, 1, 1, 1),
                        scanPlaneMode: "vertical",
                        scanPlaneRate: 3,
                        showThroughEllipsoid: !1,
                        showLateralSurfaces: !1,
                        showDomeSurfaces: !1
                    })
                })
            }
        }, {
            key: "update",
            value: function (e) {
                this.viewShadowMap && e.shadowMaps.push(this.viewShadowMap)
            }
        }, {
            key: "destroy",
            value: function () {
                this._unbindMourseEvent(),
                    this.viewer.scene.postProcessStages.remove(this.postProcess),
                    this.viewer.entities.remove(this.radar),
                    delete this.radar,
                    delete this.postProcess,
                    delete this.viewShadowMap,
                    delete this.verticalAngle,
                    delete this.viewer,
                    delete this.horizontalAngle,
                    delete this.visibleAreaColor,
                    delete this.hiddenAreaColor,
                    delete this.distance,
                    delete this.frustumQuaternion,
                    delete this.cameraPosition,
                    delete this.viewPosition,
                    delete this.alpha
            }
        }, {
            key: "horizontalAngle",
            get: function () {
                return this._horizontalAngle
            },
            set: function (e) {
                this._horizontalAngle = e,
                    this.resetRadar()
            }
        }, {
            key: "verticalAngle",
            get: function () {
                return this._verticalAngle
            },
            set: function (e) {
                this._verticalAngle = e,
                    this.resetRadar()
            }
        }, {
            key: "distance",
            get: function () {
                return this._distance
            },
            set: function (e) {
                this._distance = e, this.resetRadar()
            }
        }, {
            key: "visibleAreaColor",
            get: function () {
                return this._visibleAreaColor
            },
            set: function (e) {
                this._visibleAreaColor = e
            }
        }, {
            key: "hiddenAreaColor",
            get: function () {
                return this._hiddenAreaColor
            }, set: function (e) { this._hiddenAreaColor = e }
        }, {
            key: "alpha",
            get: function () {
                return this._alpha
            },
            set: function (e) {
                this._alpha = e
            }
        }]), e
    }()
})()